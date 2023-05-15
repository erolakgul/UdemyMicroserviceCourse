﻿namespace FreeCourse.Services.Catalog.Settings
{
    public class CustomDatabaseSettings : ICustomDatabaseSettings
    {
        public string? CourseCollectionName { get; set; }
        public string? CategoryCollectionName { get; set; }
        public string? FeatureCollectionName { get; set; }
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}
