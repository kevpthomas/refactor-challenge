﻿using System;

namespace RefactorThis.ApiModels
{
    public class ProductDto : ProductUpdateDto
    {
        public Guid Id { get; set; }
    }
}