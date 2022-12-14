// This software is part of the LittleBlocks.Ef Library
// Copyright (C) 2022 Little Blocks
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

namespace LittleBlocks.Ef.Extensions;

public static class PrincipalExtensions
{
    public const string AnonymousUser = "Anonymous";

    public static string GetUserName(this IPrincipal principal)
    {
        var identity = principal?.Identity;
        if (identity == null || identity.IsAuthenticated == false)
            return AnonymousUser;

        if (!string.IsNullOrWhiteSpace(identity.Name))
            return identity.Name;

        if (!(identity is ClaimsIdentity claimsIdentity))
            return AnonymousUser;

        var claim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        return claim?.Value ?? AnonymousUser;
    }
}
