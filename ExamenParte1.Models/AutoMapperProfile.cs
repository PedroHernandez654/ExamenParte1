using AutoMapper;
using ExamenParte1.Models.DTO;
using ExamenParte1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenParte1.Models
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Articulos,AgregarArticuloDTO>();
            CreateMap<AgregarArticuloDTO, Articulos>();

            CreateMap<GetArticulosDTO, Articulos>();
            CreateMap<Articulos, GetArticulosDTO>();

        }
    }
}
