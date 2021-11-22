﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ashirvad.Data.Model
{
    public class WebsiteModel
    {

        public List<BranchCourseEntity> branchCoursesData { get; set; } = new List<BranchCourseEntity>();
        public List<BranchClassEntity> branchClassData { get; set; } = new List<BranchClassEntity>();
        public List<BranchSubjectEntity> branchSubjectData { get; set; } = new List<BranchSubjectEntity>();
        public List<LibraryImageEntity> libraryImageEntities { get; set; } = new List<LibraryImageEntity>();
        public List<LibraryEntity1> Videolist { get; set; } = new List<LibraryEntity1>();
        public List<CategoryEntity> Categorylist { get; set; } = new List<CategoryEntity>();
        public List<LibraryEntity1> imagelist { get; set; } = new List<LibraryEntity1>();
        public List<BranchEntity> branchEntities { get; set; } = new List<BranchEntity>();
        public BranchEntity branchEntity { get; set; } = new BranchEntity();
        public List<AboutUsEntity> aboutUs { get; set; } = new List<AboutUsEntity>();


    }
}
