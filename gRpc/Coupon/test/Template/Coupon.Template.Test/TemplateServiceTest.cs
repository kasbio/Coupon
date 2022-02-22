using System.Collections;
using AutoFixture;
using AutoMapper;
using Coupon.Template.ApplicationCore.Services;
using Coupon.Template.Grpc.Model;
//using Coupon.Template.Infrastructure.Data;
using Coupon.Template.Model;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;

namespace Coupon.Template.Test;

public class TemplateServiceTest
{
    private static Fixture fixture = new Fixture();
    //private ITemplateService CreateDefaultTemplateService()
    //{
    //    var config = new MapperConfiguration(
    //        cfg =>
    //            cfg.CreateMap<CouponTemplate, CouponTemplateDTO>());
    //    IMapper mapper = new Mapper(config);
    //    Mock<ILogger<ITemplateService>> stublogger = new Mock<ILogger<ITemplateService>>();
    //    var logger = stublogger.Object;
    //}

    //private TemplateDbContext CreateDbContext()
    //{
    //    Mock<TemplateDbContext> mockDbContext = new Mock<TemplateDbContext>();
    //    mockDbContext.Setup(x => x.Templates)
    //        .ReturnsDbSet();
        
    //}

    //private IEnumerable<CouponTemplate> CreateFakeData()
    //{
    //    fixture.Build<CouponTemplate>()
    //        .With(o => o.Id, 1)
    //        .With(o => o.UserId, 123);
    //}
    
    
    
    
    #region GetTemplateAsync Test

    public void GetTemplateAsync_InvalidIdThrowException()
    {
        
    }

    #endregion

    #region GetAllByAvailableAndExpiredAsync Test

    

    #endregion


    #region FindTemplateAsync Test

    

    #endregion


    #region BuildTemplateAsync Test

    

    #endregion
}