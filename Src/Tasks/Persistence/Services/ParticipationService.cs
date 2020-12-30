﻿using Core.Application.Helper.Strategies.Participation;
using Core.Application.Interfaces;
using Core.Application.Models.Participation;
using Core.Application.Models.Participation.GETSpecificResponses;
using Core.Domain.Constants;
using Core.Domain.DbEntities;
using Infrastructure.Persistence.Strategies.Participation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;
using Core.Application.Helper.Exceptions.Participation;

namespace Infrastructure.Persistence.Services
{
    public class ParticipationService : IParticipationService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected ILogger<ParticipationService> _logger;
        protected readonly UserManager<ApplicationUser> _userManager;

        public ParticipationService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<ParticipationService> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<ParticipationResponseModel> AddNewParticipation(long createdByUserId, NewParticipationModel newParticipation)
        {
            if(newParticipation.RoleId <= Enums.ProjectRoles.None)
            {
                throw new ParticipationServiceException(ProjectParticipationRelatedErrorsConstants.CANNOT_CREATE_PARTICIPATION_WITH_NONE_AS_A_ROLE);
            }

            if(newParticipation.RoleId == Enums.ProjectRoles.Owner)
            {
                throw new ParticipationServiceException(ProjectParticipationRelatedErrorsConstants.CANNOT_CREATE_PARTICIPATION_WITH_OWNER_AS_A_ROLE);
            }

            await using var transaction = await _unitOfWork.CreateTransaction();

            try
            {
                // Check if userId in model is valid or not
                ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == newParticipation.UserId);
                if (validUser == null)
                {
                    throw new ParticipationServiceException(UserRelatedErrorsConstants.USER_NOT_FOUND);
                }

                // Check if ProjectId is valid or not
                Project validProject = _unitOfWork.Repository<Project>().GetDbset().FirstOrDefault(e => e.Id == newParticipation.ProjectId);
                if (validProject == null)
                {
                    throw new ParticipationServiceException(ProjectRelatedErrorsConstants.PROJECT_NOT_FOUND);
                }

                // In the future, we will check if the user creating the participation have the rights to create one
                // Preferably, we want only the owner, PM or leader to have the rights
                bool creatingUserHaveTheRights = _unitOfWork.Repository<UserProjects>().GetDbset()
                    .Any(item => 
                    // Check if the creator have the rights
                    item.ProjectId == validProject.Id && item.UserId == createdByUserId && 
                    item.RoleId >= Enums.ProjectRoles.Owner && item.RoleId <= Enums.ProjectRoles.Leader);
                if (!creatingUserHaveTheRights)
                {
                    throw new ParticipationServiceException(ProjectParticipationRelatedErrorsConstants.PARTICIPATION_CREATOR_DONT_HAVE_THE_RIGHTS);
                }

                // We continue to check if the new participation for the userId already exists or not
                bool newParticipationAlreadyExists = _unitOfWork.Repository<UserProjects>().GetDbset()
                    .Any(item => item.ProjectId == validProject.Id && item.UserId == validUser.UserId && item.RoleId == newParticipation.RoleId);
                if(newParticipationAlreadyExists)
                {
                    throw new ParticipationServiceException(ProjectParticipationRelatedErrorsConstants.CANNOT_RECREATE_AN_EXISTING_PARTICIPATION);
                }

                // If everything is fine, we insert the participation
                UserProjects participation = new UserProjects()
                {
                    ProjectId = validProject.Id,
                    UserId = validUser.UserId,
                    RoleId = newParticipation.RoleId.Value
                };

                await _unitOfWork.Repository<UserProjects>().InsertAsync(participation);

                await _unitOfWork.SaveChangesAsync();

                // Eager load instance for initialization of response model
                var entry = _unitOfWork.Entry(participation);
                await entry.Reference(e => e.ProjectRole).LoadAsync();

                await transaction.CommitAsync();

                return new ParticipationResponseModel(participation);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, ErrorLoggingMessagesConstants.PARTICIPATION_SERVICE_ERROR_LOG_MESSAGE);
                throw ex;
            }
        }

        public async Task<IGetAllParticipations_ResponseModel> GetAllParticipations(long queriedByUserId, GetAllParticipationsModel model)
        {
            await using var transaction = await _unitOfWork.CreateTransaction();

            try
            {
                // We use different strategies for each type of get participations request
                // Each strategy will implement its own way of data validation and returns a different response structure
                int fieldsProvided = 0;
                if (model.UserId != null)
                {
                    fieldsProvided |= (1 << 0);
                }
                if (model.ProjectId != null)
                {
                    fieldsProvided |= (1 << 1);
                }

                GetAllParticipationStrategy strategy = null;

                switch(fieldsProvided)
                {
                    // Strategy to get all participated projects of a user, and his roles in each of them
                    case (int)Enums.GetAllParticipationsStrategy.GetAllParticipatedProjects_OfUser:
                        strategy = new GetAllParticipatedProjects_OfUser_Strategy(_unitOfWork, _userManager);
                        break;
                    // Strategy to get all participated users of a project, and the roles of each of them in the project
                    case (int)Enums.GetAllParticipationsStrategy.GetAllParticipatedUsers_InProject:
                        strategy = new GetAllParticipatingUsers_InProject_Strategy(_unitOfWork, _userManager);
                        break;
                    // Strategy to get all roles of a user inside a certain project
                    case (int)Enums.GetAllParticipationsStrategy.GetAllProjectRoles_OfUser_InProject:
                        strategy = new GetProjectRoles_OfUser_InProject_Strategy(_unitOfWork, _userManager);
                        break;
                }
                IGetAllParticipations_ResponseModel result = null;
                if (strategy != null)
                {
                   result = strategy.GetAllParticipations(queriedByUserId, model);
                }
                else
                {
                    throw new Exception(InternalServerErrorsConstants.GET_ALL_PARTICIPATIONS_STRATEGY_INVALID);
                }

                await _unitOfWork.SaveChangesAsync();

                await transaction.CommitAsync();

                return result;
            } 
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, ErrorLoggingMessagesConstants.PARTICIPATION_SERVICE_ERROR_LOG_MESSAGE);
                throw ex;
            }
        }

        public Task<ParticipationResponseModel> DeleteExistingParticipation(long deletedByUserId, DeleteParticipationModel model)
        {
            throw new NotImplementedException();
        }
    }
}