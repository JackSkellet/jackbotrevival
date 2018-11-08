using Discord;
using Discord.Commands;
using Discord.WebSocket;
using jack.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace jack.Modules
{
    [Name("selfrole")]
    public class SelfRole : ModuleBase<SocketCommandContext>
    {
        private SelfRoleService _selfRoleService;
        public SelfRole(SelfRoleService roleService)
        {
            _selfRoleService = roleService;
        }
        [Command("asar", RunMode = RunMode.Async), Summary("Adds the specified role to the self assignable roles"),Remarks("asar rolename")]
        [Alias("addrole")]
        public async Task addRoleToList([Summary("Name of role to add"), Remainder]string roleName)
        {
            var user = (Context.User as SocketGuildUser);
            if (user.GuildPermissions.Has(GuildPermission.ManageRoles) || user.GuildPermissions.Has(GuildPermission.Administrator))
            {
                await _selfRoleService.AddRoleToList(Context, roleName);
            }
            else
            {
                await Context.Channel.SendMessageAsync(":no_entry_sign: To add roles to the self-assignable role list you need `Manage Roles` permissions!");
            }
        }
        [Command("iam", RunMode = RunMode.Async), Summary("Adds the specified role to yourself"),Remarks("iam rolename")]
        [Alias("iAm")]
        public async Task IAmRole([Summary("Name of role to add"), Remainder]string roleName)
        {
            await _selfRoleService.IAmRole(Context, roleName);
        }
        [Command("iamnot", RunMode = RunMode.Async), Summary("Removes the specified role from yourself"),Remarks("iamnot rolename")]
        [Alias("iAmNot")]
        public async Task IAmNotRole([Summary("Name of role to add"), Remainder]string roleName)
        {
            await _selfRoleService.IAmNotRole(Context, roleName);
        }
        [Command("rsar", RunMode = RunMode.Async), Summary("Removes the specified role from the self-assignable roles"),Remarks("rsar rolename")]
        [Alias("removerole", "delrole")]
        public async Task RemoveRoleFromList([Summary("Name of role to remove"), Remainder]string roleName)
        {
            var user = (Context.User as SocketGuildUser);
            if (user.GuildPermissions.Has(GuildPermission.ManageRoles) || user.GuildPermissions.Has(GuildPermission.Administrator))
            {
                await _selfRoleService.RemoveRoleFromList(Context, roleName);
            }
            else
            {
                await Context.Channel.SendMessageAsync(":no_entry_sign: To remove roles from the self-assignable role list you need `Manage Roles` permissions!");
            }
        }
        [Command("lsar"), Summary("Posts a list of all self-assignable roles in the Guild"),Remarks("lsar")]
        [Alias("getroles")]
        public async Task getSelfAssignableRoles()
        {
            await _selfRoleService.GetRolesInGuild(Context);
        }
        
    }
}