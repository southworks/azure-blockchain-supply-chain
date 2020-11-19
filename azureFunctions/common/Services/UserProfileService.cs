using Nethereum.KeyStore;
using Nethereum.KeyStore.Model;
using Nethereum.Signer;
using SupplyChain.Common.DataAccess;
using SupplyChain.Common.Exceptions;
using SupplyChain.Common.Models;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SupplyChain.Common.Services
{
    public class UserProfileService : IUserProfileService
    {
        private IUserProfileRepository _userProfileRepository;

        private IWalletPasswordRepository _walletPasswordRepository;

        private IKeyStoreService<ScryptParams> _keyStoreService;

        public UserProfileService(IUserProfileRepository userProfileRepository, IWalletPasswordRepository walletPasswordRepository, IKeyStoreService<ScryptParams> keyStoreService)
        {
            _userProfileRepository = userProfileRepository;
            _walletPasswordRepository = walletPasswordRepository;
            _keyStoreService = keyStoreService;
        }

        public async Task<string> CreateAndUploadUserProfile(string name, string email)
        {
            if (!ValidateUserEmail(email))
            {
                throw new Exception("Invalid user profile field: email must be a valid email address.");
            }

            // Generate wallet and encrypted keystore
            var key = EthECKey.GenerateKey();
            var privateKey = key.GetPrivateKeyAsBytes();
            var address = key.GetPublicAddress();
            var password = Guid.NewGuid().ToString();

            var keyStoreObj = _keyStoreService.EncryptAndGenerateKeyStore(password, privateKey, address);

            var userProfile = new UserProfile
            {
                Name = name,
                Email = email,
                Id = keyStoreObj.Id,
                UserKeyStore = keyStoreObj
            };

            var walletPassword = new WalletPassword
            {
                Id = Guid.NewGuid().ToString(),
                UserProfileId = userProfile.Id,
                Password = password
            };

            await _userProfileRepository.Insert(userProfile);

            await _walletPasswordRepository.Insert(walletPassword);

            return keyStoreObj.Id;
        }

        public async Task<UserProfile> GetUserProfile(string id)
        {
            var userProfile = await _userProfileRepository.GetById(id);

            if (userProfile == null)
            {
                throw new UserProfileException();
            }

            var walletPassword = await _walletPasswordRepository.GetByUserId(userProfile.Id);
            userProfile.Password = walletPassword.Password;

            return userProfile;
        }

        public async Task DeleteUserProfile(string id)
        {
            var userProfile = await _userProfileRepository.GetById(id);

            if (userProfile == null)
            {
                throw new UserProfileException();
            }

            ValidateUser(userProfile);

            await _userProfileRepository.Delete(id);
        }

        public async Task<bool> UpdateUserProfile(string id, UserProfile userProfile, string newName, string newEmail)
        {
            if (newName != null)
            {
                userProfile.Name = newName;
            }

            if (newEmail != null)
            {
                userProfile.Email = newEmail;
            }

            if (newName != null || newEmail != null)
            {
                if (!ValidateUserEmail(userProfile.Email))
                {
                    throw new Exception("Invalid user profile field: email must be a valid email address.");
                }

                await _userProfileRepository.Update(id, userProfile);

                return true;
            }

            return false;
        }

        private async void ValidateUser(UserProfile userProfile)
        {
            var walletPassword = await _walletPasswordRepository.GetByUserId(userProfile.Id);

            _keyStoreService.DecryptKeyStore(walletPassword.Password, userProfile.UserKeyStore);
        }

        private bool ValidateUserEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
