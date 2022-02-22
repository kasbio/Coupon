using Kasbio.Coupon.Template.ApplicationCore.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kasbio.Coupon.Template.ApplicationCore.Command
{
   public class ConstructCouponByTemplateCommand:IRequest 
    {
        private CouponTemplate _template;
        public ConstructCouponByTemplateCommand(CouponTemplate template)
        {
            this._template = template;
        }
 

        public CouponTemplate Template
        {
            get { return _template; }
         
        }

    }
}
