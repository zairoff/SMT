using AutoMapper;
using CoreHtmlToImage;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using SMT.Access.Migrations;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Notification;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.HourlyPlanDto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class HourlyPlanService : IHourlyPlanService
    {
        private readonly IHourlyPlanRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly long _chatId;

        public HourlyPlanService(IMapper mapper, IUnitOfWork unitOfWork, IHourlyPlanRepository repository, INotificationService notificationService, IConfiguration configuration)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _notificationService = notificationService;

            _chatId = Convert.ToInt64(configuration["AppSettings:HourlyPlanChatId"]);
        }

        public async Task<HourlyPlanResponse> AddAsync(HourlyPlanCreate hourlyPlanCreate)
        {
            var hourlyPlan = _mapper.Map<HourlyPlanCreate, HourlyPlan>(hourlyPlanCreate);

            await _repository.AddAsync(hourlyPlan);
            await _unitOfWork.SaveAsync();

            hourlyPlan = await _repository.FindAsync(m => m.Id == hourlyPlan.Id);

            return _mapper.Map<HourlyPlan, HourlyPlanResponse>(hourlyPlan);
        }

        public async Task<HourlyPlanResponse> DeleteAsync(int id)
        {
            var hourlyPlan = await _repository.FindAsync(p => p.Id == id);

            if (hourlyPlan == null)
                throw new NotFoundException("HourlyPlan not found");

            _repository.Delete(hourlyPlan);
            await _unitOfWork.SaveAsync();

            await NotifyAsync("O'CHIRILDI");

            return _mapper.Map<HourlyPlan, HourlyPlanResponse>(hourlyPlan);
        }

        public async Task<IEnumerable<HourlyPlanResponse>> GetAllAsync()
        {
            var hourlyPlans = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<HourlyPlan>, IEnumerable<HourlyPlanResponse>>(hourlyPlans);
        }

        public async Task<HourlyPlanResponse> GetAsync(int id)
        {
            var hourlyPlan = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<HourlyPlan, HourlyPlanResponse>(hourlyPlan);
        }

        public async Task<IEnumerable<HourlyPlanResponse>> GetByDate(DateTime date)
        {
            var hourlyPlans = await _repository.GetByAsync(p => p.Time.Date == date.Date);

            return _mapper.Map<IEnumerable<HourlyPlan>, IEnumerable<HourlyPlanResponse>>(hourlyPlans);
        }

        public async Task<IEnumerable<HourlyPlanResponse>> GetByLineId(int lineId)
        {
            var hourlyPlans = await _repository.GetByAsync(p => p.LineId == lineId);

            return _mapper.Map<IEnumerable<HourlyPlan>, IEnumerable<HourlyPlanResponse>>(hourlyPlans);
        }

        public async Task<IEnumerable<HourlyPlanResponse>> GetByModelId(int modelId)
        {
            var hourlyPlans = await _repository.GetByAsync(p => p.ModelId == modelId);

            return _mapper.Map<IEnumerable<HourlyPlan>, IEnumerable<HourlyPlanResponse>>(hourlyPlans);
        }

        public async Task<IEnumerable<HourlyPlanResponse>> GetByBrandId(int brandId)
        {
            var hourlyPlans = await _repository.GetByAsync(p => p.Model.ProductBrand.BrandId == brandId);

            return _mapper.Map<IEnumerable<HourlyPlan>, IEnumerable<HourlyPlanResponse>>(hourlyPlans);
        }

        public async Task<IEnumerable<HourlyPlanResponse>> GetByProductId(int productId)
        {
            var hourlyPlans = await _repository.GetByAsync(p => p.Model.ProductBrand.ProductId == productId);

            return _mapper.Map<IEnumerable<HourlyPlan>, IEnumerable<HourlyPlanResponse>>(hourlyPlans);
        }

        public async Task<HourlyPlanResponse> UpdateAsync(int id, HourlyPlanUpdate hourlyPlanUpdate)
        {
            var hourlyPlan = await _repository.FindAsync(p => p.Id == id);


            if (hourlyPlan == null)
                throw new NotFoundException("Not found");

            hourlyPlan.LineId = hourlyPlanUpdate.LineId;
            hourlyPlan.ModelId = hourlyPlanUpdate.ModelId;
            hourlyPlan.Produced = hourlyPlanUpdate.Produced;
            hourlyPlan.Plan = hourlyPlanUpdate.Plan;

            _repository.Update(hourlyPlan);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<HourlyPlan, HourlyPlanResponse>(hourlyPlan);
        }

        public async Task<IEnumerable<HourlyPlanResponse>> GetByLineAndDate(int lineId, DateTime from, DateTime to)
        {
            var hourlyPlans = await _repository.GetByAsync(p => p.LineId == lineId && p.Time >= from && p.Time <= to);

            return _mapper.Map<IEnumerable<HourlyPlan>, IEnumerable<HourlyPlanResponse>>(hourlyPlans);
        }

        public async Task NotifyAsync()
        {
            await NotifyAsync("KIRITILDI");
        }

        private async Task NotifyAsync(string title)
        {
            var hourlyPlans = await _repository.GetByAsync(x => x.Time.Date == DateTime.Now.Date);

            var body = BuildColumns2(hourlyPlans);
            
            var html = BuildBody(body, title);

            var memory = ConvertHtmlToImage(html);

            await _notificationService.NotifyAsync(memory, title, _chatId);
        }

        private static StringBuilder BuildColumns2(IEnumerable<HourlyPlan> hourlyPlans)
        {
            var groupByLine = hourlyPlans.GroupBy(x => x.Line.Name).OrderByDescending(x => x.Count());

            var length = hourlyPlans.GroupBy(x => new { x.ModelId, x.LineId }).Max(x => x.Count());

            var body = new StringBuilder();

            foreach (var lineGroup in groupByLine)
            {
                var groupByModel = lineGroup.GroupBy(x => x.ModelId).OrderByDescending(x => x.Count());

                foreach (var modelGroup in groupByModel)
                {
                    var columns = new StringBuilder();
                    var rows = new StringBuilder();
                    var table = new StringBuilder();

                    table.AppendLine("<div>");
                    table.AppendLine($"<p>{lineGroup.Key}</p>");
                    table.AppendLine("<table>");

                    columns.AppendLine("<tr>");
                    columns.AppendLine("<th>MODEL</th>");
                    columns.AppendLine("<th>REJA</th>");
                    columns.AppendLine("<th>FARQI</th>");
                    columns.AppendLine("<th>%</th>");

                    var hourly = modelGroup.First();
                    var sum = modelGroup.Sum(x => x.Produced);
                    var difference = sum - hourly.Plan;
                    var actualDifference = difference > 0 ? $"+{difference}" : $"{difference}";
                    var percentage = hourly.Plan > 0 ? Math.Abs((sum * 100) / hourly.Plan) : 100;

                    rows.AppendLine("<tr>");
                    rows.AppendLine($"<td>{hourly.Model.Name}</td>");
                    rows.AppendLine($"<td>{hourly.Plan}</td>");
                    rows.AppendLine($"<td>{actualDifference}</td>");
                    rows.AppendLine($"<td>{percentage}%</td>");

                    foreach (var hourlyPlan in modelGroup)
                    {
                        columns.AppendLine($"<th>{hourlyPlan.Time:HH:mm}</th>");
                        rows.AppendLine($"<td>{hourlyPlan.Produced}</td>");
                    }

                    for (int index = modelGroup.Count(); index < length; index++)
                    {
                        columns.AppendLine($"<th> </th>");
                        rows.AppendLine($"<td> </td>");
                    }

                    columns.AppendLine("<th>UMUMIY</th>");
                    columns.AppendLine("</tr>");

                    rows.AppendLine($"<td>{sum}</td>");
                    rows.AppendLine("</tr>");

                    table.AppendLine(columns.ToString());
                    table.AppendLine(rows.ToString());

                    table.AppendLine("</table>");
                    table.AppendLine("</div>");

                    body.AppendLine(table.ToString());
                }
            }



            return body;
        }

        private static string BuildBody(StringBuilder body, string title)
        {
            return $@"<!DOCTYPE html>
                        <html>
                        <head>
                        <style>
                        table {{
                          font-family: arial, sans-serif;
                          border-collapse: collapse;
                          table-layout: fixed;
                          width: 1500px
                        }}

                        td, th {{
                          border: 1px solid #dddddd;
                          text-align: center;
                          padding: 8px;
                        }}
                        </style>
                        </head>
                            <body>
                                <h2>{title}</h2>
                                {body}
                            </body>
                        </html>";
        }

        private static MemoryStream ConvertHtmlToImage(string html)
        {
            var converter = new HtmlConverter();
            var bytes = converter.FromHtmlString(html);
            return new MemoryStream(bytes);
        }
    }
}
