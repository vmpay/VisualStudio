﻿using Template.Objects;
using System;
using System.Linq;

namespace Template.Services
{
    public interface IRoleService : IService
    {
        void SeedPrivilegesTree(RoleView view);

        IQueryable<RoleView> GetViews();
        RoleView GetView(String id);

        void Create(RoleView view);
        void Edit(RoleView view);
        void Delete(String id);
    }
}
