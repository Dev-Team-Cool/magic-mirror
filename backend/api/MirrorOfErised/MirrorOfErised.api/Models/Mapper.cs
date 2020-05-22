using Microsoft.AspNetCore.Identity;
using MirrorOfErised.models.Repos;
using Project.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MirrorOfErised.api.Models
{
    public static class Mapper
    {
        public static AuthToken_DTO ConvertTo_DTO(AuthToken @event, ref AuthToken_DTO AuthToken_DTO)
        {

            AuthToken_DTO.ExpireDate = @event.ExpireDate;
            AuthToken_DTO.RefreshToken = @event.RefreshToken.ToString();
            AuthToken_DTO.Token = @event.Token.ToString();
            AuthToken_DTO.UserName = @event.UserName.ToString();


            return AuthToken_DTO;
        }

        /*public static async Task<AuthToken> ConvertTo_Entity(AuthToken_DTO AuthToken_DTO, AuthTokenRepo eventRepo , UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IdentityUser Loggedin, AuthToken @event)
        {
            @event.EventStartDate = DateTime.Parse(AuthToken_DTO.EventStartDate);
            @event.EventSubscribeEnd = DateTime.Parse(AuthToken_DTO.EventSubscribeEnd);
            @event.EventEndDate = DateTime.Parse(AuthToken_DTO.EventEndDate);
            *//*//Date is een string in het DTO => Parse
            @event.EventName = toDoTask_DTO.EventName;*//*
            //2. velden die NULL kunnen zijn (Bij create AND
            @event.EventDiscription = @event.EventDiscription ?? AuthToken_DTO.EventDiscription;
            @event.EventMaxParticipants = AuthToken_DTO.EventMaxParticipants;
            @event.EventName = AuthToken_DTO.EventName;
            @event.EventPlace = AuthToken_DTO.EventPlace;
            @event.EventId = Guid.NewGuid().ToString();




            await eventRepo.Add(@event);
            IdentityResult roleResult;
            string[] roleNames = { "Admin", "Host", "Subscriber", "Unsubscriber", "Extra" };


            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    Role pRole = new Role(roleName);
                    pRole.EventId = @event.EventId;
                    pRole.Name = roleName + @event.EventName.Replace(" ", "");
                    roleResult = await roleManager.CreateAsync(pRole);
                }
            }

            string Name = "Host" + @event.EventName;


            var userResult = await userManager.AddToRoleAsync(Loggedin, Name);


            return @event;


        }
        public static Event UpdateEvent(IEventRepo eventRepo, Event @event,ref AuthToken_DTO AuthToken_DTO)
        {
            @event.EventStartDate = DateTime.Parse(AuthToken_DTO.EventStartDate);
            @event.EventSubscribeEnd = DateTime.Parse(AuthToken_DTO.EventSubscribeEnd);
            @event.EventEndDate = DateTime.Parse(AuthToken_DTO.EventEndDate);
            @event.EventDiscription = @event.EventDiscription ?? AuthToken_DTO.EventDiscription;
            @event.EventMaxParticipants = AuthToken_DTO.EventMaxParticipants;
            @event.EventName = AuthToken_DTO.EventName;
            @event.EventPlace = AuthToken_DTO.EventPlace;

            eventRepo.Update(@event);

            return @event;
        }*/
    }
    }