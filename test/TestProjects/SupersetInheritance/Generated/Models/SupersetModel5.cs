// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.ResourceManager.Core;

namespace SupersetInheritance
{
    /// <summary> The SupersetModel5. </summary>
    public partial class SupersetModel5 : SupersetModel4Data
    {
        /// <summary> Initializes a new instance of SupersetModel5. </summary>
        /// <param name="location"> The location. </param>
        public SupersetModel5(LocationData location) : base(location)
        {
        }

        /// <summary> Initializes a new instance of SupersetModel5. </summary>
        /// <param name="id"> The id. </param>
        /// <param name="name"> The name. </param>
        /// <param name="type"> The type. </param>
        /// <param name="location"> The location. </param>
        /// <param name="tags"> The tags. </param>
        /// <param name="new"> . </param>
        /// <param name="foo"> . </param>
        internal SupersetModel5(ResourceGroupResourceIdentifier id, string name, ResourceType type, LocationData location, IDictionary<string, string> tags, string @new, string foo) : base(id, name, type, location, tags, @new)
        {
            Foo = foo;
        }

        public string Foo { get; set; }
    }
}
