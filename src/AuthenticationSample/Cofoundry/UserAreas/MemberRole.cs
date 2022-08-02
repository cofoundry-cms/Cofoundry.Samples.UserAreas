﻿namespace AuthenticationSample;

/// <summary>
/// Roles can be defined in code as well as in the admin panel. Defining
/// a role in code means that it gets added automatically at startup. 
/// Additionally we have a RoleCode that we can use to query 
/// the role programatically.
/// 
/// See https://www.cofoundry.org/docs/framework/roles-and-permissions
/// </summary>
public class MemberRole : IRoleDefinition
{
    /// <summary>
    /// By convention we add a constant for the role code
    /// to make it easier to reference.
    /// </summary>
    public const string Code = "MEM";

    /// <summary>
    /// The role code is a unique three letter code that can be used to reference 
    /// the role programatically. The code must be unique and convention is to use 
    /// upper case, although code matching is case insensitive.
    /// </summary>
    public string RoleCode => Code;

    /// <summary>
    /// The role title is used to identify the role and select it in the admin 
    /// UI and therefore must be unique. Max 50 characters.
    /// </summary>
    public string Title => "Member";

    /// <summary>
    /// A role must be assigned to a user area, in this case the role is used for members
    /// </summary
    public string UserAreaCode => MemberUserArea.Code;

    /// <summary>
    /// This method determines which permissions the role is granted when it is first created. 
    /// To help do this you are provided with a builder that contains all permissions in the 
    /// system which you can use to either include or exclude permissions based on rules.
    /// </summary>
    public void ConfigurePermissions(IPermissionSetBuilder builder)
    {
        // We need to add in permissions to update and delete the current user to
        // support these features
        builder
            .ApplyAnonymousRoleConfiguration()
            .IncludeCurrentUser(c => c.Update().Delete());
    }
}