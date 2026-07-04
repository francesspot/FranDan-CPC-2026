using FranDanBackend;
using FranDanBackend.DTO;
using FranDanBackend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.SecurityTokenService;
using System.Globalization;
using System.IO;
using System.Numerics;

namespace FranDanBackend.Services
{
    public class PlanService
    {
        public MyContext context {  get; set; }
        public PlanService(MyContext _context)
        {
            context = _context;
        }

        public void create(int userId, PlanCreateDTO dto)
        {
            User user = context.getUserById(userId);
            Plan newPlan = new Plan(dto.title,dto.category,dto.description,dto.startTime,user);
            context.Plans.Add(newPlan);
            Participation creatorParticipation = new Participation(user, newPlan, true);
            creatorParticipation.accepted = true;
            context.Participations.Add(creatorParticipation);
            context.SaveChanges();
        }
        public void edit(int userId, PlanEditDTO dto)
        {
            User user = context.getUserById(userId);
            Plan plan = context.getPlanById(dto.id);
            switch (context.getParticipationRole(user, plan))
            {
                case Participation.Role.CREATOR: break;
                case Participation.Role.ADMIN: break;
                case Participation.Role.INVITED_ADMIN: throw new Exception("Accept invitation before editing plan.");
                case Participation.Role.PARTICIPANT: throw new Exception("You have to be admin to edit the plan.");
                case Participation.Role.INVITED: throw new Exception("First join the plan. You have to be admin to edit the plan.");
                case Participation.Role.NONE: throw new Exception("You are not included in this plan.");
            }
            plan.title = dto.title;
            plan.category = dto.category;
            plan.description = dto.description;
            plan.startTime = DateTime.Parse(dto.startTime, CultureInfo.GetCultureInfo("pl-PL"));
            context.SaveChanges();
        }
        public void addParticipant(int userId, PlanParticipantDTO dto)
        {
            User user = context.getUserById(userId);
            User invitedUser = context.getUserById(dto.userId);
            Plan plan = context.getPlanById(dto.planId);
            switch(context.getParticipationRole(user, plan))
            {
                case Participation.Role.CREATOR: break;
                case Participation.Role.ADMIN: if(dto.admin) throw new Exception("You have to be creator to set admin role."); break;
                case Participation.Role.INVITED_ADMIN: throw new Exception("Accept invitation before inviting others.");
                case Participation.Role.PARTICIPANT: throw new Exception("You have to be admin to add to the plan.");
                case Participation.Role.INVITED: throw new Exception("First join the plan. You have to be admin to add to the plan.");
                case Participation.Role.NONE: throw new Exception("You are not included in this plan.");
            }
            if (context.getParticipationRole(invitedUser, plan)!=Participation.Role.NONE) throw new Exception("Already a participant");
            Participation participation = new Participation(invitedUser, plan, dto.admin);
            context.Participations.Add(participation);
            context.SaveChanges();
            if (invitedUser.emailNotifications) MailSender.sendPlanRequest(user, invitedUser, plan);
        }
        public void removeParticipant(int userId, PlanActionDTO dto)
        {
            User user = context.getUserById(userId);
            User toRemoveUser = context.getUserById(dto.userId);
            Plan plan = context.getPlanById(dto.planId);
            switch (context.getParticipationRole(user, plan))
            {
                case Participation.Role.CREATOR:
                    switch (context.getParticipationRole(toRemoveUser, plan))
                    {
                        case Participation.Role.CREATOR: throw new Exception("You can't quit plan as a creator.");
                        case Participation.Role.ADMIN: break;
                        case Participation.Role.INVITED_ADMIN: break;
                        case Participation.Role.PARTICIPANT: break;
                        case Participation.Role.INVITED: break;
                        case Participation.Role.NONE: throw new Exception("Not a participant.");
                    }
                    break;
                case Participation.Role.ADMIN:
                    switch (context.getParticipationRole(toRemoveUser, plan))
                    {
                        case Participation.Role.CREATOR: throw new Exception("You have no authority to remove the creator.");
                        case Participation.Role.ADMIN:
                            if (user == toRemoveUser) break;
                            else throw new Exception("You have no authority to remove another admin.");
                        case Participation.Role.INVITED_ADMIN: throw new Exception("You have no authority to remove another admin.");
                        case Participation.Role.PARTICIPANT: break;
                        case Participation.Role.INVITED: break;
                        case Participation.Role.NONE: throw new Exception("Not a participant.");
                    }
                    break;
                case Participation.Role.INVITED_ADMIN: throw new Exception("Join the plan before removing participants.");
                case Participation.Role.PARTICIPANT:
                    if (user == toRemoveUser) break;
                    else throw new Exception("You have to be admin to remove from the plan.");
                case Participation.Role.INVITED: throw new Exception("Join the plan first. You have to be admin to remove from the plan.");
                case Participation.Role.NONE: throw new Exception("You are not included in this plan.");
            }
            Participation participation = context.getParticipation(toRemoveUser, plan);
            context.Participations.Remove(participation);
            context.SaveChanges();
        }
        public void acceptInvitation(int userId, PlanIdDTO dto)
        {
            User user = context.getUserById(userId);
            Plan plan = context.getPlanById(dto.id);
            Participation participation = context.getParticipation(user, plan);
            if (!participation.accepted)
            {
                participation.accepted=true;
            }
            else
            {
                throw new Exception("Already in the plan");
            }
            context.SaveChanges();
        }
        public void rejectInvitation(int userId, PlanIdDTO dto)
        {
            User user = context.getUserById(userId);
            Plan plan = context.getPlanById(dto.id);
            Participation participation = context.getParticipation(user, plan);
            if (participation.accepted)
                throw new Exception("Already in the plan");
            context.Participations.Remove(participation);
            context.SaveChanges();
        }
        public void setAdmin(int userId, PlanParticipantDTO dto)
        {
            User user = context.getUserById(userId);
            User toChangeStateUser = context.getUserById(dto.userId);
            Plan plan = context.getPlanById(dto.planId);
            switch (context.getParticipationRole(user, plan))
            {
                case Participation.Role.CREATOR: if(user == toChangeStateUser) throw new Exception("Can't change own creator state."); break;
                case Participation.Role.ADMIN: if (user == toChangeStateUser) break; throw new Exception("No authority to set admin.");
                case Participation.Role.NONE: throw new Exception("You are not included in this plan.");
                default: throw new Exception("No authority to set admin.");
            }
            Participation participation = context.getParticipation(toChangeStateUser, plan);
            if(participation.admin == dto.admin) throw new Exception("Can't change to the same admin state");
            participation.admin = dto.admin;
            context.SaveChanges();
        }
        public PlanFullDTO fullPlan(int userId, PlanIdDTO dto)
        {
            User user = context.getUserById(userId);
            Plan plan = context.getPlanById(dto.id);
            var participantsList = context.getAllPlanParticipants(plan);
            List<PlanMemberDTO> members = new List<PlanMemberDTO>();
            foreach (var (participant, accepted, creator, admin) in participantsList)
            {
                members.Add(participant.toPlanMemberDTO(accepted,admin));
            }
            return new PlanFullDTO
            {
                id = plan.id,
                title = plan.title,
                category = plan.category,
                description = plan.description,
                startTime = plan.startTime.ToString("yyyy-MM-dd HH.mm.ss"),
                creator = plan.creator.toPlanMemberDTO(true, true),
                participants=members

            };
        }
        public void delete(int userId, PlanIdDTO dto)
        {
            User user = context.getUserById(userId);
            Plan plan = context.getPlanById(dto.id);
            if(plan.creator!=user) throw new Exception("No authority to delete this plan"); 
            context.Plans.Remove(plan);
            context.SaveChanges();
        }

    }
}
