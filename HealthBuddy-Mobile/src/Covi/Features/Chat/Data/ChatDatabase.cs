using Covi.Features.Chat.Components;
using Covi.Features.RapidProFcmPushNotifications.Services;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Covi.Features.Chat.Data
{
    public class ChatDatabase
    {
        private readonly SQLiteAsyncConnection _dataBase;
        private readonly RapidProContainer _rapidProContainer;
        private readonly FirebaseContainer _firebaseContainer;

        public ChatDatabase()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "healthbuddypluschat_db.db3");
            _dataBase = new SQLiteAsyncConnection(dbPath);
            _dataBase.CreateTableAsync<RapidProMessage>().Wait();
            _rapidProContainer = new RapidProContainer();
            _firebaseContainer = new FirebaseContainer();
        }

        public Task<List<RapidProMessage>> GetRapidProMessagesAsync()
        {
            //Get all messages.
            return _dataBase.Table<RapidProMessage>().ToListAsync();
        }

        public Task<List<RapidProMessage>> GetRapidProMessagesByChannelIdAsync(string channelId)
        {
            //Get all messages.
            return _dataBase.Table<RapidProMessage>().Where(x => x.ChannelId == channelId).OrderBy(o => o.CreatedDateTime).ToListAsync();
        }

        public Task<RapidProMessage> GetRapidProMessageAsync(string id)
        {
            // Get a specific message.
            return _dataBase.Table<RapidProMessage>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> InsertRapidProMessageAsync(RapidProMessage message)
        {
            // Save a new message.
            return _dataBase.InsertAsync(message);
        }

        public Task<int> UpdateRapidProMessageAsync(RapidProMessage message)
        {
            // Update an existing message.
            return _dataBase.UpdateAsync(message);
        }

        public Task<int> DeleteRapidProMessageAsync(RapidProMessage message)
        {
            // Delete a message.
            return _dataBase.DeleteAsync(message);
        }

        public Task<int> DeleteRapidProMessageQuickReplieAsync(RapidProMessage message)
        {
            // Delete a message.
            return _dataBase.DeleteAsync(message);
        }

        public Task<int> DeleteAllRapidProMessageAsync()
        {
            _firebaseContainer.FirebaseChannelId = string.Empty;
            _firebaseContainer.FirebaseChannelHost = string.Empty;
            _rapidProContainer.RapidProIsChatDatabase = false;
            _rapidProContainer.RapidProIsInit = false;
            _rapidProContainer.RapidProIsInitMsg = false;
            // Delete all message.
            return _dataBase.DeleteAllAsync<RapidProMessage>();
        }

        public async Task<bool> DeleteAllAnonymousRapidProMessageAsync()
        {
            //Get all messages.
            var messages = await _dataBase.Table<RapidProMessage>().ToListAsync();

            if (messages.Any())
            {
                //Get Anonymous User all messages.
                var userAnonymousMessages = messages.Where(x => x.User == MessageUserEnum.UserAnonymous.ToDescriptionAttr());

                if (userAnonymousMessages.Any())
                {
                    foreach (var userAnonymousMessage in userAnonymousMessages)
                    {
                        _firebaseContainer.FirebaseChannelId = string.Empty;
                        _firebaseContainer.FirebaseChannelHost = string.Empty;
                        _rapidProContainer.RapidProIsInit = false;
                        _rapidProContainer.RapidProIsInitMsg = false;
                        // Delete a message.
                        _dataBase.DeleteAsync(userAnonymousMessage);
                    }
                }
            }

            return true;
        }
    }
}
