IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Roles] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [Description] nvarchar(300) NOT NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
    [CreatedBy] int NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] int NULL,
    [DeactivatedAt] datetime2 NULL,
    [DeactivatedBy] int NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Email] nvarchar(100) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [FirstName] nvarchar(100) NOT NULL,
    [LastName] nvarchar(100) NOT NULL,
    [ImageUrl] nvarchar(max) NULL,
    [DocumentType] nvarchar(20) NULL,
    [DocumentNumber] nvarchar(20) NULL,
    [BirthDate] datetime2 NULL,
    [Phone] nvarchar(20) NULL,
    [Mobile] nvarchar(20) NULL,
    [Gender] nvarchar(20) NULL,
    [Address] nvarchar(200) NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    [IsBlocked] bit NOT NULL DEFAULT CAST(0 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
    [CreatedBy] int NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] int NULL,
    [DeactivatedAt] datetime2 NULL,
    [DeactivatedBy] int NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE TABLE [RoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] int NOT NULL,
    [ClaimType] nvarchar(50) NOT NULL,
    [ClaimValue] nvarchar(100) NOT NULL,
    [IsEnabled] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
    [CreatedBy] int NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] int NULL,
    [DeactivatedAt] datetime2 NULL,
    [DeactivatedBy] int NULL,
    CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RoleClaims_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [UserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [ClaimType] nvarchar(50) NOT NULL,
    [ClaimValue] nvarchar(100) NOT NULL,
    [IsEnabled] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
    [CreatedBy] int NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] int NULL,
    [DeactivatedAt] datetime2 NULL,
    [DeactivatedBy] int NULL,
    CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserClaims_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [UserRefreshTokens] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [JwtId] nvarchar(max) NOT NULL,
    [RefreshTokenHash] nvarchar(max) NOT NULL,
    [IsRevoked] bit NOT NULL DEFAULT CAST(0 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
    [ExpirationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_UserRefreshTokens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserRefreshTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [UserRoles] (
    [UserId] int NOT NULL,
    [RoleId] int NOT NULL,
    [IsAssigned] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
    [CreatedBy] int NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] int NULL,
    [DeactivatedAt] datetime2 NULL,
    [DeactivatedBy] int NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_RoleClaims_RoleId] ON [RoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [IX_Roles_Name] ON [Roles] ([Name]);

CREATE INDEX [IX_UserClaims_UserId] ON [UserClaims] ([UserId]);

CREATE INDEX [IX_UserRefreshTokens_UserId] ON [UserRefreshTokens] ([UserId]);

CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);

CREATE UNIQUE INDEX [IX_Users_DocumentNumber] ON [Users] ([DocumentNumber]) WHERE [DocumentNumber] IS NOT NULL;

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251116193131_CreateInitialScheme', N'9.0.10');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251118231746_AddUserRoleAuditFields', N'9.0.10');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Roles]') AND [c].[name] = N'Description');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Roles] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Roles] ALTER COLUMN [Description] nvarchar(300) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251122011623_UpdateNullableEntities', N'9.0.10');

CREATE TABLE [Permissions] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(50) NOT NULL,
    [Description] nvarchar(200) NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETDATE()),
    [CreatedBy] int NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] int NULL,
    [DeactivatedAt] datetime2 NULL,
    [DeactivatedBy] int NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY ([Id])
);

CREATE TABLE [Menus] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(100) NOT NULL,
    [Route] nvarchar(200) NOT NULL,
    [ParentId] int NULL,
    [Order] int NOT NULL DEFAULT 0,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    [PermissionId] int NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] int NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] int NULL,
    [DeactivatedAt] datetime2 NULL,
    [DeactivatedBy] int NULL,
    CONSTRAINT [PK_Menus] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Menus_Menus_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Menus] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Menus_Permissions_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permissions] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [RolePermissions] (
    [RoleId] int NOT NULL,
    [PermissionId] int NOT NULL,
    [IsEnabled] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] int NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] int NULL,
    [DeactivatedAt] datetime2 NULL,
    [DeactivatedBy] int NULL,
    CONSTRAINT [PK_RolePermissions] PRIMARY KEY ([RoleId], [PermissionId]),
    CONSTRAINT [FK_RolePermissions_Permissions_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permissions] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_RolePermissions_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [UserPermissions] (
    [UserId] int NOT NULL,
    [PermissionId] int NOT NULL,
    [IsEnabled] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] int NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] int NULL,
    [DeactivatedAt] datetime2 NULL,
    [DeactivatedBy] int NULL,
    CONSTRAINT [PK_UserPermissions] PRIMARY KEY ([UserId], [PermissionId]),
    CONSTRAINT [FK_UserPermissions_Permissions_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permissions] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserPermissions_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Menus_ParentId] ON [Menus] ([ParentId]);

CREATE INDEX [IX_Menus_PermissionId] ON [Menus] ([PermissionId]);

CREATE UNIQUE INDEX [IX_Menus_Route] ON [Menus] ([Route]);

CREATE UNIQUE INDEX [IX_Permissions_Name] ON [Permissions] ([Name]);

CREATE INDEX [IX_RolePermissions_PermissionId] ON [RolePermissions] ([PermissionId]);

CREATE INDEX [IX_UserPermissions_PermissionId] ON [UserPermissions] ([PermissionId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251123161425_AddPermissionScheme', N'9.0.10');

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserPermissions]') AND [c].[name] = N'CreatedAt');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [UserPermissions] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [UserPermissions] ADD DEFAULT (GETDATE()) FOR [CreatedAt];

ALTER TABLE [RolePermissions] ADD [CreatedByUserId] int NULL;

ALTER TABLE [RolePermissions] ADD [DeactivatedByUserId] int NULL;

ALTER TABLE [RolePermissions] ADD [UpdateByUserId] int NULL;

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Menus]') AND [c].[name] = N'CreatedAt');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Menus] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Menus] ADD DEFAULT (GETDATE()) FOR [CreatedAt];

CREATE INDEX [IX_Users_CreatedBy] ON [Users] ([CreatedBy]);

CREATE INDEX [IX_Users_DeactivatedBy] ON [Users] ([DeactivatedBy]);

CREATE INDEX [IX_Users_UpdatedBy] ON [Users] ([UpdatedBy]);

CREATE INDEX [IX_UserRoles_CreatedBy] ON [UserRoles] ([CreatedBy]);

CREATE INDEX [IX_UserRoles_DeactivatedBy] ON [UserRoles] ([DeactivatedBy]);

CREATE INDEX [IX_UserRoles_UpdatedBy] ON [UserRoles] ([UpdatedBy]);

CREATE INDEX [IX_UserPermissions_CreatedBy] ON [UserPermissions] ([CreatedBy]);

CREATE INDEX [IX_UserPermissions_DeactivatedBy] ON [UserPermissions] ([DeactivatedBy]);

CREATE INDEX [IX_UserPermissions_UpdatedBy] ON [UserPermissions] ([UpdatedBy]);

CREATE INDEX [IX_UserClaims_CreatedBy] ON [UserClaims] ([CreatedBy]);

CREATE INDEX [IX_UserClaims_DeactivatedBy] ON [UserClaims] ([DeactivatedBy]);

CREATE INDEX [IX_UserClaims_UpdatedBy] ON [UserClaims] ([UpdatedBy]);

CREATE INDEX [IX_Roles_CreatedBy] ON [Roles] ([CreatedBy]);

CREATE INDEX [IX_Roles_DeactivatedBy] ON [Roles] ([DeactivatedBy]);

CREATE INDEX [IX_Roles_UpdatedBy] ON [Roles] ([UpdatedBy]);

CREATE INDEX [IX_RolePermissions_CreatedByUserId] ON [RolePermissions] ([CreatedByUserId]);

CREATE INDEX [IX_RolePermissions_DeactivatedByUserId] ON [RolePermissions] ([DeactivatedByUserId]);

CREATE INDEX [IX_RolePermissions_UpdateByUserId] ON [RolePermissions] ([UpdateByUserId]);

CREATE INDEX [IX_RoleClaims_CreatedBy] ON [RoleClaims] ([CreatedBy]);

CREATE INDEX [IX_RoleClaims_DeactivatedBy] ON [RoleClaims] ([DeactivatedBy]);

CREATE INDEX [IX_RoleClaims_UpdatedBy] ON [RoleClaims] ([UpdatedBy]);

CREATE INDEX [IX_Permissions_CreatedBy] ON [Permissions] ([CreatedBy]);

CREATE INDEX [IX_Permissions_DeactivatedBy] ON [Permissions] ([DeactivatedBy]);

CREATE INDEX [IX_Permissions_UpdatedBy] ON [Permissions] ([UpdatedBy]);

CREATE INDEX [IX_Menus_CreatedBy] ON [Menus] ([CreatedBy]);

CREATE INDEX [IX_Menus_DeactivatedBy] ON [Menus] ([DeactivatedBy]);

CREATE INDEX [IX_Menus_UpdatedBy] ON [Menus] ([UpdatedBy]);

ALTER TABLE [Menus] ADD CONSTRAINT [FK_Menus_Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Menus] ADD CONSTRAINT [FK_Menus_Users_DeactivatedBy] FOREIGN KEY ([DeactivatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Menus] ADD CONSTRAINT [FK_Menus_Users_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Permissions] ADD CONSTRAINT [FK_Permissions_Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Permissions] ADD CONSTRAINT [FK_Permissions_Users_DeactivatedBy] FOREIGN KEY ([DeactivatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Permissions] ADD CONSTRAINT [FK_Permissions_Users_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [RoleClaims] ADD CONSTRAINT [FK_RoleClaims_Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [RoleClaims] ADD CONSTRAINT [FK_RoleClaims_Users_DeactivatedBy] FOREIGN KEY ([DeactivatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [RoleClaims] ADD CONSTRAINT [FK_RoleClaims_Users_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [RolePermissions] ADD CONSTRAINT [FK_RolePermissions_Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [Users] ([Id]);

ALTER TABLE [RolePermissions] ADD CONSTRAINT [FK_RolePermissions_Users_DeactivatedByUserId] FOREIGN KEY ([DeactivatedByUserId]) REFERENCES [Users] ([Id]);

ALTER TABLE [RolePermissions] ADD CONSTRAINT [FK_RolePermissions_Users_UpdateByUserId] FOREIGN KEY ([UpdateByUserId]) REFERENCES [Users] ([Id]);

ALTER TABLE [Roles] ADD CONSTRAINT [FK_Roles_Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Roles] ADD CONSTRAINT [FK_Roles_Users_DeactivatedBy] FOREIGN KEY ([DeactivatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Roles] ADD CONSTRAINT [FK_Roles_Users_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [UserClaims] ADD CONSTRAINT [FK_UserClaims_Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [UserClaims] ADD CONSTRAINT [FK_UserClaims_Users_DeactivatedBy] FOREIGN KEY ([DeactivatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [UserClaims] ADD CONSTRAINT [FK_UserClaims_Users_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [UserPermissions] ADD CONSTRAINT [FK_UserPermissions_Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [UserPermissions] ADD CONSTRAINT [FK_UserPermissions_Users_DeactivatedBy] FOREIGN KEY ([DeactivatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [UserPermissions] ADD CONSTRAINT [FK_UserPermissions_Users_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [UserRoles] ADD CONSTRAINT [FK_UserRoles_Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [UserRoles] ADD CONSTRAINT [FK_UserRoles_Users_DeactivatedBy] FOREIGN KEY ([DeactivatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [UserRoles] ADD CONSTRAINT [FK_UserRoles_Users_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Users] ADD CONSTRAINT [FK_Users_Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Users] ADD CONSTRAINT [FK_Users_Users_DeactivatedBy] FOREIGN KEY ([DeactivatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Users] ADD CONSTRAINT [FK_Users_Users_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251126172035_UpdateAuditableEntities', N'9.0.10');

DROP TABLE [Menus];

DROP TABLE [UserPermissions];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251129014404_RemoveMenuSchema', N'9.0.10');

COMMIT;
GO

