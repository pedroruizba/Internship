﻿using AutoMapper;
using Internship.API.Models;
using Internship.API.Repositories.Interfaces;
using Internship.API.Services.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internship.API.Services
{
    public class InternService : IInternService
    {
        private readonly IInternRepository _internRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public InternService(IInternRepository internRepository, IUserRepository userRepository, IMapper mapper)
        {
            _internRepository = internRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }



        public InternDTO Create(InternDTO internDTO)
        {
            Intern intern = new Intern();
            User user = _userRepository.GetById(internDTO.UserId);
            User mentorId = _userRepository.GetById(internDTO.MentorId);
            if (user != null && mentorId != null && _internRepository.Get(internDTO.UserId) == null)
            {
                //Map all info from internDTO to intern
                intern = _mapper.Map<Intern>(internDTO);
                //Call create method to store the data, and assign the result in the intern variable
                intern = _internRepository.Create(intern);
                //Map all info from the result to userDTO
                internDTO = _mapper.Map<InternDTO>(intern);
                //load user info
                //internDTO.Load user Info(user);
                internDTO.LoadUserInfo(user);
                User mentor = _userRepository.GetById(internDTO.MentorId);
                //load mentor info
                //internDTO.Load menor Info(user);
                internDTO.LoadMentorInfo(mentor);
                /*if (internDTO.MentorId != null)
                {
                    User mentor = _userRepository.GetById(internDTO.MentorId);
                    //load mentor info
                    //internDTO.Load menor Info(user);
                    internDTO.LoadMentorInfo(mentor);
                }*/

                //return created intern of type InternDTO
                return internDTO;
            }
            else
            {
                internDTO = null;
            }
            return internDTO;
        }

        public InternDTO GetInternById(string id)
        {
            //Map all info from userDTO to user
            Intern getIntern = _internRepository.GetInternById(id);
            //Map all info from the result to userDTO
            InternDTO internDTO = _mapper.Map<InternDTO>(getIntern);
            //return the user of type UserDTO
            if (internDTO.UserId != null)
            {
                User user = _userRepository.GetById(internDTO.UserId);
                //load mentor info
                //internDTO.Load menor Info(user);
                internDTO.LoadUserInfo(user);
            }
            if (internDTO.MentorId != null)
            {
                User mentor = _userRepository.GetById(internDTO.MentorId);
                //load mentor info
                //internDTO.Load menor Info(user);
                internDTO.LoadMentorInfo(mentor);
            }
            return internDTO;
        }
    }
}


