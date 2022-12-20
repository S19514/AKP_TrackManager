﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace AKP_TrackManager.Models.DTO
{
    public class MemberCreateDto
    {
        public Member Member { get; set; }
        public string ModelErrorString { get; set; }
        public string ModelErrorKey { get; set; }
        public SelectList ViewData { get; set; }
    }
}
