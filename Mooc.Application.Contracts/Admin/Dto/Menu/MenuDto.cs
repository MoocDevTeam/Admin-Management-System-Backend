﻿using Mooc.Model.Entity;
using Mooc.Shared.Enum;

namespace Mooc.Application.Contracts.Admin
{
    public class MenuDto: BaseEntityDto
    {
        public string? Title { get; set; }
        public string? Permission { get; set; }
        public MenuType MenuType { get; set; }
        public string? Description { get; set; }
        public int OrderNum { get; set; } = 0;
        public string? Route { get; set; }
        public string? ComponentPath { get; set; }
        public virtual ICollection<MenuDto> Children { get; set; } = new List<MenuDto>();
        public long? ParentId { get; set; }

        public Menu? Parent { get; set; }
        public virtual ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();
    }

}
