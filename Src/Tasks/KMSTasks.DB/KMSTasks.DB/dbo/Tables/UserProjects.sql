﻿CREATE TABLE [dbo].[UserProjects](
	[UserId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[RoleId] [tinyint] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserProjects]  WITH CHECK ADD  CONSTRAINT [FK_UserProjects_AspNetUsers1] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([UserId])
GO

ALTER TABLE [dbo].[UserProjects] CHECK CONSTRAINT [FK_UserProjects_AspNetUsers1]
GO
ALTER TABLE [dbo].[UserProjects]  WITH CHECK ADD  CONSTRAINT [FK_UserProjects_Project1] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([Id])
GO

ALTER TABLE [dbo].[UserProjects] CHECK CONSTRAINT [FK_UserProjects_Project1]
GO
ALTER TABLE [dbo].[UserProjects]  WITH CHECK ADD  CONSTRAINT [FK_UserProjects_ProjectRole] FOREIGN KEY([RoleId])
REFERENCES [dbo].[ProjectRoles] ([Id])
GO

ALTER TABLE [dbo].[UserProjects] CHECK CONSTRAINT [FK_UserProjects_ProjectRole]
GO
ALTER TABLE [dbo].[UserProjects] ADD  DEFAULT (CONVERT([tinyint],(0))) FOR [RoleId]