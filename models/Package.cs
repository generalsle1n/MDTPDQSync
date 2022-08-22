using System;
using System.Collections.Generic;

namespace MDTPDQSync.models
{
    public partial class Package
    {
        public long PackageId { get; set; }
        public long? FolderId { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
        public string? Description { get; set; }
        public string? Version { get; set; }
        public long? PackageDefinitionId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public long? LibraryPackageVersionId { get; set; }
        public long? CurrentLibraryPackageVersionId { get; set; }
        public long? NewLibraryPackageVersionId { get; set; }
        public bool? IsAutoDownload { get; set; }
        public long? OriginalId { get; set; }
    }
}
