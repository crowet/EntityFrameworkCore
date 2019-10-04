// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal
{
    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public static class NavigationExtensions
    {
        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public static string ToDebugString(
            [NotNull] this INavigation navigation,
            bool singleLine = true,
            bool includeIndexes = false,
            [NotNull] string indent = "")
        {
            var builder = new StringBuilder();

            builder.Append($"{indent}Navigation: {navigation.DeclaringEntityType.DisplayName()}.{navigation.Name}");

            if (includeIndexes)
            {
                var indexes = navigation.GetPropertyIndexes();
                builder.Append(" ").Append(indexes.Index);
                builder.Append(" ").Append(indexes.OriginalValueIndex);
                builder.Append(" ").Append(indexes.RelationshipIndex);
                builder.Append(" ").Append(indexes.ShadowIndex);
                builder.Append(" ").Append(indexes.StoreGenerationIndex);
            }

            if (!singleLine)
            {
                builder.Append(navigation.AnnotationsToDebugString(indent + "  "));
            }

            return builder.ToString();
        }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public static MemberIdentity CreateMemberIdentity([CanBeNull] this INavigation navigation)
            => navigation?.GetIdentifyingMemberInfo() == null
                ? MemberIdentity.Create(navigation?.Name)
                : MemberIdentity.Create(navigation.GetIdentifyingMemberInfo());

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public static Navigation AsNavigation([NotNull] this INavigation navigation, [NotNull] [CallerMemberName] string methodName = "")
            => MetadataExtensions.AsConcreteMetadataType<INavigation, Navigation>(navigation, methodName);
    }
}
