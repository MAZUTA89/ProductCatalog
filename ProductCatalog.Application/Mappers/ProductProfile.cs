using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using AutoMapper.Configuration;
using ProductCatalog.Application.DTO;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Domain.Core.Entities;

namespace ProductCatalog.Application.Mappers
{
    public class ProductProfile : Profile 
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(
                d => d.Title,
                src => src.MapFrom(s => s.Title))
                .ForMember(
                s => s.Description,
                d => d.MapFrom(dto => dto.Description));

            CreateMap<ProductImage, ProductImageDto>()
                .ForMember(
                d => d.FileName,
                src => src.MapFrom(s => s.FileName));

            CreateMap<Product, ProductCsvDto>()
                .ForMember(
                d => d.Id,
                s => s.MapFrom(src => src.Id))
                .ForMember(
                d => d.Title,
                s => s.MapFrom(src => src.Title))
                .ForMember(
                d => d.Description,
                s => s.MapFrom(src => src.Description))
                .ForMember(
                d => d.ImagesAmount,
                s => s.MapFrom(src => src.Images.Count));

            CreateMap<CreateProductCommand, Product>();

            CreateMap<FileContent, ProductImage>()
                .ForMember(
                d => d.FileName,
                src => src.MapFrom(s => s.FileName));
                
            CreateMap<Product, ResultProductDto>()
                .ForMember(
                d => d.ProductId,
                src => src.MapFrom(s => s.Id))
                .ForMember(
                d => d.Title,
                src => src.MapFrom(s => s.Title))
                .ForMember(
                s => s.Description,
                d => d.MapFrom(dto => dto.Description));
        }
    }
}
