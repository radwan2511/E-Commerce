using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace SadnaExpress.DomainLayer.User
{
    public class UserFacade : IUserFacade
    {
        //Dictionary<int, Guest> guests;
        ConcurrentDictionary<int, User> Current_Users;
        ConcurrentDictionary<int, Member> Members;
        private int USER_ID = 0;

        PasswordHash _ph = new PasswordHash();

        public UserFacade()
        {
            //guests = new Dictionary<int, Guest>();
            Current_Users = new ConcurrentDictionary<int, User>();
            Members = new ConcurrentDictionary<int, Member>();
        }

        public int Enter()
        {
            User user = new User(USER_ID);
            USER_ID++;
            Current_Users.TryAdd(USER_ID, user);

            Logger.Info(user ,"Enter the system.");


            return user.UserId;
        }

        public void Exit(int id)
        {
            User user;
            Current_Users.TryRemove(id, out user);
            Logger.Info(user ,"exited from the system.");
        }

        public void Register(int id, string email, string firstName, string lastName, string password)
        {
            if (Current_Users.ContainsKey(id))
                throw new SadnaException("user with this id already logged in", "UserFacade", "Register");

            string hashPassword = _ph.Hash(password);
            Member newMember = new Member(id, email, firstName, lastName, hashPassword);
            newMember.LoggedIn = false;
            Members.TryAdd(id, newMember);

            Logger.Info(newMember ,"registered with "+email+".");
        }

        public int Login(int id, string email, string password)
        {
            foreach (Member member in Members.Values)
            {
                if (!member.Email.Equals(email))
                {
                    if (!member.Password.Equals(_ph.Hash(password))){ //need to check
                        throw new SadnaException("wrong password for email", "UserFacade", "Login");
                    }
                    else
                    {
                        //correct email & password:
                        member.LoggedIn = true;
                        User user;
                        Current_Users.TryRemove(id, out user);
                        Logger.Info(member, "logged in");

                        return member.UserId;
                    }
                }
            }
            //eamil not found
            throw new SadnaException("email dosen't exist", "UserFacade", "Login");
        }

        public int Logout(int id)
        {
            if (!Members.ContainsKey(id))
                throw new SadnaException("member with id dosen't exist", "UserFacade", "Logout");

            // todo save shopping cart

            Member member = Members[id];
            member.LoggedIn = false;
            Logger.Info(member, "logged out");
            return Enter(); //member logs out and a regular user enters the system instead
        }

        public void AddItemToBag(int id, string storeName, string itemName)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, List<string>> getDetailsOnCart()
        {
            throw new NotImplementedException();
        }

        public void PurchaseCart(int id)
        {
            throw new NotImplementedException();
        }

        public void AddItemCart(int id, string storeName, string itemName)
        {
            throw new NotImplementedException();
        }

        public void RemoveCart(int id, string storeName, string itemName)
        {
            throw new NotImplementedException();
        }

        public void EditItemCart(int id, string storeName, string itemName)
        {
            throw new NotImplementedException();
        }

        public void OpenStore(int id, string storeName)
        {
            throw new NotImplementedException();
        }

        public void AddReview(int id, string storeName, string itemName)
        {
            throw new NotImplementedException();
        }

        public void AddItemInventory(int id, string storeName, string itemName)
        {
            throw new NotImplementedException();
        }

        public void RemoveItemInventory(int id, string storeName, string itemName)
        {
            throw new NotImplementedException();
        }

        public void EditItemInventory(int id, string storeName, string itemName)
        {
            throw new NotImplementedException();
        }

        public void AddOwner(int id, string storeName, string email)
        {
            throw new NotImplementedException();
        }

        public void AddManager(int id, string storeName, string email)
        {
            throw new NotImplementedException();
        }

        public void AddPermissionsToManager(int id, string storeName, string email, string Permission)
        {
            throw new NotImplementedException();
        }

        public void CloseStore(int id, string storeName)
        {
            throw new NotImplementedException();
        }

        public void GetDetailsOnStore(int id, string storeName)
        {
            throw new NotImplementedException();
        }
    }
}