using AutoMapper;
using ExamenParte1.DataBase;
using ExamenParte1.Models;
using ExamenParte1.Models.DTO;
using ExamenParte1.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenParte1.Services
{
    public class ArticuloService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ArticuloService()
        {
            _context = new ApplicationDbContext();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            _mapper = config.CreateMapper();
        }

        public async Task<List<GetArticulosDTO>> GetArticulos()
        {
            var getArticulosDb = _context.Articulos.ToList();

            var productosDto = _mapper.Map<List<GetArticulosDTO>>(getArticulosDb);

            return productosDto;
        }

        public async Task<(GetArticulosDTO,string)> GetArticulo(string codigoSKU)
        {
            var getArticulosDb = await _context.Articulos.Where(x => x.CodigoSKU == codigoSKU).FirstOrDefaultAsync();

            if(getArticulosDb == null)
            {
                return (new GetArticulosDTO(), "Código SKU no encontrado");
            }

            var productosDto = _mapper.Map<GetArticulosDTO>(getArticulosDb);

            return (productosDto,string.Empty);
        }

        public async Task<(bool success, string response)> AgregarArticulo(AgregarArticuloDTO nuevoArticulo)
        {
            var existArticulo = await _context.Articulos.Where(x => x.CodigoSKU == nuevoArticulo.CodigoSKU).FirstOrDefaultAsync();

            if (existArticulo != null)
            {
                return (false, "Código SKU previamente creado");
            }


            using (var transaccion = _context.Database.BeginTransaction())
            {
                try
                {
                    var dbModelo = _mapper.Map<Articulos>(nuevoArticulo);
                    if (dbModelo.GeneraImpuesto)
                    {
                        dbModelo.PrecioVenta = dbModelo.PrecioUnitario * 1.16m;
                    }
                    else
                    {
                        dbModelo.PrecioVenta = dbModelo.PrecioUnitario;
                    }

                    dbModelo.Total = (dbModelo.PrecioVenta * dbModelo.Existencia);

                    await _context.Articulos.AddAsync(dbModelo);
                    await _context.SaveChangesAsync();
                    transaccion.Commit();
                    return (true, "Articulo Añadido");

                }
                catch (Exception ex)
                {
                    transaccion.Rollback();

                    return (false, ex.Message);
                }
            }
        }

        public async Task<(bool success, string response)> EditarArticulo(AgregarArticuloDTO articuloDTO)
        {
            var existArticulo = await _context.Articulos.Where(x => x.CodigoSKU == articuloDTO.CodigoSKU).FirstOrDefaultAsync();

            if (existArticulo != null)
            {
                using (var transaccion = _context.Database.BeginTransaction())
                {
                    try
                    {
                    
                        var dbModelo = _mapper.Map(articuloDTO, existArticulo);

                        if (articuloDTO.GeneraImpuesto != existArticulo.GeneraImpuesto)
                        {
                            if (articuloDTO.GeneraImpuesto && !existArticulo.GeneraImpuesto)
                            {
                                dbModelo.PrecioVenta = dbModelo.PrecioUnitario * 1.16m;
                            }
                            else if (!articuloDTO.GeneraImpuesto && existArticulo.GeneraImpuesto)
                            {
                                dbModelo.PrecioVenta = existArticulo.PrecioVenta / (1 + 0.16m);
                            }
                        }

                        dbModelo.Total = dbModelo.PrecioVenta * dbModelo.Existencia;

                        _context.Entry(existArticulo).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        transaccion.Commit();
                        return (true, "Articulo Editado");

                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback();

                        return (false, ex.Message);
                    }
                }
            }
            else
            {
                return (false, "Código SKU no encontrado");
            }
        }

        public async Task<(bool success, string response)> EliminarArticulo(string codigoSKU)
        {
            var existArticulo = await _context.Articulos.Where(x => x.CodigoSKU == codigoSKU).FirstOrDefaultAsync();

            if (existArticulo == null)
            {
                return (false, "Código SKU no encontrado");
            }

            if (existArticulo.Existencia > 0)
            {
                return (false, "Aún existe este articulo");

            }

            using (var transaccion = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.Articulos.Remove(existArticulo);
                    await _context.SaveChangesAsync();
                    await transaccion.CommitAsync();
                    return (true, "Artículo eliminado");
                }
                catch (Exception ex)
                {
                    await transaccion.RollbackAsync();
                    return (false, ex.Message);
                }
            }
        }


    }
}
