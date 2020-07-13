﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.CodeAnalysis
{
    internal partial struct SymbolKey
    {
        private static class FieldSymbolKey
        {
            public static void Create(IFieldSymbol symbol, SymbolKeyWriter visitor)
            {
                visitor.WriteString(symbol.MetadataName);
                visitor.WriteSymbolKey(symbol.ContainingType);
            }

            public static SymbolKeyResolution Resolve(SymbolKeyReader reader, out string failureReason)
            {
                var metadataName = reader.ReadString();
                var containingTypeResolution = reader.ReadSymbolKey(out var containingTypeFailureReason);
                if (containingTypeFailureReason != null)
                {
                    failureReason = $"({nameof(FieldSymbolKey)} {nameof(containingTypeResolution)} failed -> ${containingTypeFailureReason})";
                    return default;
                }

                using var result = GetMembersOfNamedType<IFieldSymbol>(containingTypeResolution, metadataName);
                return CreateResolution(result, $"({nameof(FieldSymbolKey)} '{metadataName}' not found)", out failureReason);
            }
        }
    }
}
