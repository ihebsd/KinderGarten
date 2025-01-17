﻿using Solution.Data;
using Solution.Domain.Entities;
using Solution.Service;
using Solution.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Solution.Web.Controllers
{
    public class EventWebApiController : ApiController
    {

        IEventService MyService = null;
        private EventService es = new EventService();
        List<EventModel> events = new List<EventModel>();

        public EventWebApiController()
        {
            MyService = new EventService();
            Index();
            events = Index().ToList();
        }
        public List<EventModel> Index()
        {
            List<Event> mandates = es.GetMany().ToList();
            List<EventModel> mandatesXml = new List<EventModel>();
            foreach (Event i in mandates)
            {
                mandatesXml.Add(new EventModel
                {
                    EventId = i.EventId,
                    DateEvent = i.DateEvent,
                    Name = i.Name,
                    Description = i.Description,
                    number_P = i.number_P,
                    Category = i.Category,
                    HeureD = i.HeureD,
                    HeureF = i.HeureF,
                    image = i.image,
                    qrCode = i.qrCode


                });
            }
            return mandatesXml;
        }
        // GET api/EventWebApi
        [HttpGet]
        public IEnumerable<EventModel> Get()
        {
            return events;
        }

        // GET api/<controller>/5
        public IEnumerable<EventModel> Get(int id)
        {
            Event i = MyService.GetById(id);

            return new List<EventModel>() { new EventModel() {
                EventId = i.EventId,
                    DateEvent = i.DateEvent,
                    Name = i.Name,
                    Description = i.Description,
                    number_P = i.number_P,
                    Category = i.Category,
                    HeureD = i.HeureD,
                    HeureF= i.HeureF    ,
                    image=i.image,
                    qrCode = i.qrCode

            }
            };
        }

        // POST: api/EventWebApi
        [Route("api/EventPost")]
        public IHttpActionResult PostNewFeed(EventModel postt)
        {
            using (var ctx = new PidevContext())
            {
                ctx.Events.Add(new Event()
                {
                    DateEvent = postt.DateEvent,
                    Name = postt.Name,
                    Description = postt.Description,
                    number_P = postt.number_P,
                    image = postt.image,
                    Category = postt.Category,
                    DirecteurFk = 1,
                    HeureD = postt.HeureD,
                    HeureF = postt.HeureF,
                    qrCode = postt.qrCode,

                }); ;

                ctx.SaveChanges();
            }

            return Ok();
        }

        [Route("api/EventApi/Put")]
        public IHttpActionResult Put(int id, EventModel student)
        {


            Event existingStudent = MyService.GetById(id);
            if (existingStudent != null)
            {
                existingStudent.Name = student.Name;
                existingStudent.number_P = student.number_P;
                existingStudent.Category = student.Category;
                existingStudent.Description = student.Description;
                existingStudent.image = student.image;
                existingStudent.HeureD = student.HeureD;
                existingStudent.HeureF = student.HeureF;
                existingStudent.DateEvent = student.DateEvent;

                MyService.Update(existingStudent);
                MyService.Commit();
            }
            else
            {
                return NotFound();
            }


            return Ok();
        }


        // DELETE: api/EventWebApi/5
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new PidevContext())
            {
                var student = ctx.Events
                    .Where(s => s.EventId == id)
                    .FirstOrDefault();
                ctx.Entry(student).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }
        [HttpGet]
        [Route("api/EventApi/Details")]
        public Event GetbyId(int id)
        {
            Event ev = MyService.GetById(id);

            return ev;
        }
        [HttpGet]
        [Route("api/EventApi/QrCode")]
        public IEnumerable<Event> GetByqrCode(String id)
        {
            List<Event> Kinder = new List<Event>();
            Kinder.Add(MyService.FindEventByQrCode(id));

            return Kinder;
        }
    }
}