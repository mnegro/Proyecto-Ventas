using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ventas.Models.Response;
using Ventas.Models.Request;
using Ventas.Models;
using Microsoft.AspNetCore.Authorization;

namespace Ventas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ClienteController : ControllerBase
    {
       [HttpGet]
        public IActionResult Get()
        {
            Respuesta orespuesta = new Respuesta();
            orespuesta.Exito = 0;
            try
            {
                using (VentasContext db = new VentasContext())
                {
                    var lst = db.Clientes.OrderByDescending(d=>d.Id).ToList();
                    orespuesta.Exito = 1;
                    orespuesta.Data = lst;
                }

            }catch (Exception ex)
            {
                orespuesta.Mensaje = ex.Message;
            }

            return Ok(orespuesta);
        }

        [HttpPost]
        public IActionResult Add(ClienteRequest oModel)
        {
            Respuesta orespuesta = new Respuesta();
            try
            {
                using(VentasContext db = new VentasContext())
                {
                    Cliente ocliente = new Cliente();
                    ocliente.Nombre = oModel.Nombre;
                    db.Clientes.Add(ocliente);
                    db.SaveChanges();
                    orespuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {
                orespuesta.Mensaje = ex.Message;
            }
            return Ok(orespuesta);
        }

        [HttpPut]
        public IActionResult Edit(ClienteRequest oModel)
        {
            Respuesta orespuesta = new Respuesta();
            try
            {
                using (VentasContext db = new VentasContext())
                {
                    Cliente ocliente = db.Clientes.Find(oModel.Id);
                    ocliente.Nombre = oModel.Nombre;
                    db.Entry(ocliente).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    orespuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {
                orespuesta.Mensaje = ex.Message;
            }
            return Ok(orespuesta);
        }

        [HttpDelete ("{Id}")]
        public IActionResult Delete(int Id)
        {
            Respuesta orespuesta = new Respuesta();
            try
            {
                using (VentasContext db = new VentasContext())
                {
                    Cliente ocliente = db.Clientes.Find(Id);
                    db.Remove(ocliente);
                    db.SaveChanges();
                    orespuesta.Exito = 1;
                }
            }
            catch (Exception ex)
            {
                orespuesta.Mensaje = ex.Message;
            }
            return Ok(orespuesta);
        }
    }

    
}
