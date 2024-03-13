using System.Collections;
using System.Collections.Generic;
using Habby.CNUser;
using UnityEngine;

namespace SDKFramework.Account
{
    public class Approver
    {
        public string Name;
        
        public Approver NextApprover;
        
    }

    public abstract class Approver<T> : Approver where T : Response
    {
        protected Approver(string name)
        {
            this.Name = name;
        }

        public abstract void ProcessRequest(T Request);
    }

    public class RegisterApprover : Approver<RegisterResponse>
    {
        public RegisterApprover(string name) : base(name){}
        
        public override void ProcessRequest(RegisterResponse Request)
        {
            
        }
    }

    public class LoginApprover : Approver<LoginResponse>
    {
        public LoginApprover(string name) : base(name){ }

        public override void ProcessRequest(LoginResponse Request)
        {
            
        }
    }
    
    public class IdentityApprover : Approver<IdentityResponse>
    {
        public IdentityApprover(string name) : base(name){ }

        public override void ProcessRequest(IdentityResponse Request)
        {
            
        }
    }

}