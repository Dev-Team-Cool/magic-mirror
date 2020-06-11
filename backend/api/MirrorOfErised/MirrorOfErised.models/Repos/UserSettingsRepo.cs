﻿using Microsoft.AspNetCore.Identity;
using MirrorOfErised.models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MirrorOfErised.models.Repos
{
    public class UserSettingsRepo : IUserSettingsRepo
    {
        private readonly ApplicationDbContext _context;

        public UserSettingsRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<UserSettings> AddSetting(UserSettings settings)
        {
            try
            {
                await _context.UserSettings.AddAsync(settings);
                await _context.SaveChangesAsync();

                return settings;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<UserSettings> GetSettingsForUserIdAsync(string id)
        {
            try
            {
                return await _context.UserSettings.Where(e => e.UserId == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<UserSettings> UpdateSetting(UserSettings settings)
        {
            try
            {
                _context.UserSettings.Update(settings);
                await _context.SaveChangesAsync();

                return settings;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
