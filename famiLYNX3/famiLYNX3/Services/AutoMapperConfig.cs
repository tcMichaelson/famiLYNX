using AutoMapper;
using famiLYNX3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace famiLYNX3.Services {
    public class AutoMapperConfig {
        public static void RegisterMaps() {
            Mapper.CreateMap<AddressDTO, Address>();
            Mapper.CreateMap<Address, AddressDTO>();
            Mapper.CreateMap<ConversationDTO, Conversation>();
            Mapper.CreateMap<Conversation, ConversationDTO>();
            Mapper.CreateMap<FamilyDTO, Family>();
            Mapper.CreateMap<Family, FamilyDTO>();
            Mapper.CreateMap<FamilyTypeDTO, FamilyType>();
            Mapper.CreateMap<FamilyType, FamilyTypeDTO>();
            Mapper.CreateMap<ApplicationUser, ApplicationUserDTO>();
            Mapper.CreateMap<ApplicationUserDTO, ApplicationUser>();
            Mapper.CreateMap<Message, MessageDTO>();
            Mapper.CreateMap<MessageDTO, Message>();
            Mapper.CreateMap<FamilyUser, FamilyUserDTO>();
            Mapper.CreateMap<FamilyUserDTO, FamilyUser>();
            Mapper.CreateMap<InviteOrPleaDTO, InviteOrPlea>();
            Mapper.CreateMap<InviteOrPlea, InviteOrPleaDTO>();
        }
    }
}