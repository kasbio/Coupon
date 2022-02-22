using AutoMapper;
using Coupon.Redis;
using Coupon.Template.ApplicationCore.Services;
using Coupon.Template.DbContexts;
using Coupon.Template.Grpc.Model;
using Coupon.Template.Infrastructure;
using Coupon.Template.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Coupon.Template.ApplicationCore.Implement
{
    internal class TemplateService : ITemplateService
    {
        private readonly TemplateDbContext _dbContext;
        private readonly ILogger<ITemplateService> _logger;
        private readonly IMapper _mapper;

        public TemplateService(
            ILogger<ITemplateService> logger,
            TemplateDbContext dbContext,
            IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        private IEnumerable<CouponTemplateDTO> MapTo(IEnumerable<CouponTemplate> source)
        {
            return _mapper.Map<IEnumerable<CouponTemplateDTO>>(source);
        }


        private CouponTemplateDTO MapTo(CouponTemplate source)
        {
            return _mapper.Map<CouponTemplateDTO>(source);
        }

        private CouponTemplateWithStatus NewListWithStatus(IEnumerable<CouponTemplateDTO> data)
        {
            var d = new CouponTemplateWithStatus();
            d.Data.AddRange(data);
            return d;
        }

        private CouponTemplateList NewList(IEnumerable<CouponTemplateDTO> data)
        {
            var d = new CouponTemplateList();
            d.Data.AddRange(data);
            return d;
        }

        public async Task<CouponTemplateDTO> BuildTemplateAsync(TemplateRequest request)
        {
            var template = _mapper.Map<CouponTemplate>(request);
            template.Available = true;
            await _dbContext.AddAsync(template);
            await _dbContext.SaveChangesAsync();
            string redisKey = Constant.GetCouponRedisKey(template.Id);
            _logger.LogDebug($"redis key: {redisKey}");
            var codes = BuildCouponCode(template);
            long i = RedisHelper.RPush(redisKey, codes.ToArray());
            _logger.LogDebug($"success insert coupon code : {i}");


            return MapTo(template);
        }

        public async Task<IEnumerable<CouponTemplateDTO>> FindAllUsableTemplateAsync()
        {
            var list =  await _dbContext.GetAllByAvailableAndExpiredAsync(true, false);
            return MapTo(list);
        }

        public async Task<CouponTemplateWithStatus> FindTemplateAsync(IEnumerable<int> ids)
        {
            if (ids.IsNullOrEmpty())
            {
                throw new ArgumentNullException("传入参数Ids为空");
            }

            var templates = await _dbContext.Templates
                .Where(o => ids.Contains(o.Id))
                .ToListAsync();

            var t = MapTo(templates);

            return NewListWithStatus(t);
        }

        public async Task<CouponTemplateList> GetAllByAvailableAndExpiredAsync(bool available, bool expired)
        {
            var list = await _dbContext.GetAllByAvailableAndExpiredAsync(available, expired);
            var t = MapTo(list);
            return NewList(t);
        }

        public async Task<CouponTemplateDTO> GetTempalteAsync(int id)
        {
            if (id < 0)
            {
                throw new ArgumentNullException("非法Id");
            }

            var r = await _dbContext.FindAsync<CouponTemplate>(id);
            return MapTo(r);
        }

        private HashSet<string> BuildCouponCode(CouponTemplate template)
        {


            HashSet<string> result = new HashSet<string>(template.Count);

            // 前四位
            string prefix4 = $"{(int)template.ProductLine}{(int)template.Category}";
            string date = template.CreateTime.ToString("yyMMdd");

            for (int i = 0; i != template.Count; ++i)
            {
                result.Add(prefix4 + BuildCouponCodeSuffix14(date));
            }

            while (result.Count < template.Count)
            {
                result.Add(prefix4 + BuildCouponCodeSuffix14(date));
            }
            return result;
        }



        private string BuildCouponCodeSuffix14(string date)
        {
            // 中间六位
            char[] chars = date.ToCharArray();
            //打乱
            Random r = new Random();//创建随机类对象，定义引用变量为r
            for (int i = 0; i < chars.Length; i++)
            {
                int index = r.Next(chars.Length);//随机获得0（包括0）到arr.Length（不包括arr.Length）的索引
                //Console.WriteLine("index={0}", index);//查看index的值
                char temp = chars[i];  //当前元素和随机元素交换位置
                chars[i] = chars[index];
                chars[index] = temp;
            }

            //中间六位
            string mid6 = new string(chars, 0, chars.Length);


            string suffix8 = string.Empty;
            for (int i = 0; i < 8; i++)
            {
                suffix8 += r.Next(0, 10).ToString();
            }


            return mid6 + suffix8;
        }
    }
}
