using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FF_Fastfood.Models;
namespace FF_Fastfood.Models.ChartClass
{
    public class FillChartData
    {
        public DateTime timeStartForChart { get; set; }
        public DateTime timeEndStartForChart { get; set; }
        public int? typeFilter { get; set; }

        FF_FastFoodEntities1 db = new FF_FastFoodEntities1();

        public FillChartData()
        {
            // Khởi tạo các giá trị mặc định
            this.timeStartForChart = DateTime.Today;
            this.timeEndStartForChart = DateTime.Today.AddDays(1).AddTicks(-1);
            this.typeFilter = null;
        }

        public FillChartData(FillChartData fillChart)
        {
            this.timeStartForChart = fillChart.timeStartForChart;
            this.typeFilter = fillChart.typeFilter;
            this.timeEndStartForChart = fillChart.timeEndStartForChart;
        }

        public List<DataForChart> FillChart()
        {
            List<DataForChart> dataChart = new List<DataForChart>();
            List<Order> orders = null;

            if (typeFilter == null)
            {
                // Hiển thị tất cả doanh thu
                orders = db.Orders.OrderBy(o => o.order_date).ToList();
            }
            else if (typeFilter == 0)
            {
                // Thống kê ngày hôm nay
                timeStartForChart = DateTime.Today;
                timeEndStartForChart = DateTime.Today.AddDays(1).AddTicks(-1);
                orders = db.Orders
                    .Where(x => x.order_date >= timeStartForChart && x.order_date < timeEndStartForChart)
                    .OrderBy(o => o.order_date)
                    .ToList();

                // Nhóm các đơn hàng theo giờ
                dataChart = orders
                    .GroupBy(o => o.order_date.Value.Hour)
                    .Select(g => new DataForChart
                    {
                        order_date = $"{timeStartForChart:dd/MM/yyyy} {g.Key}:00",
                        total_amount = g.Sum(o => o.total_amount)
                    })
                    .ToList();
            }
            else if (typeFilter == 1)
            {
                // Thống kê ngày hôm qua
                timeStartForChart = DateTime.Today.AddDays(-1);
                timeEndStartForChart = DateTime.Today.AddTicks(-1);
                orders = db.Orders
                    .Where(x => x.order_date >= timeStartForChart && x.order_date < timeEndStartForChart)
                    .OrderBy(o => o.order_date)
                    .ToList();

                // Nhóm các đơn hàng theo giờ
                dataChart = orders
                    .GroupBy(o => o.order_date.Value.Hour)
                    .Select(g => new DataForChart
                    {
                        order_date = $"{timeStartForChart:dd/MM/yyyy} {g.Key}:00",
                        total_amount = g.Sum(o => o.total_amount)
                    })
                    .ToList();
            }
            else if (typeFilter == 2)
            {
                // Thống kê 7 ngày gần nhất (không tính hôm nay)
                timeStartForChart = DateTime.Today.AddDays(-7);
                timeEndStartForChart = DateTime.Today.AddTicks(-1);
                orders = db.Orders
                    .Where(x => x.order_date >= timeStartForChart && x.order_date < timeEndStartForChart)
                    .OrderBy(o => o.order_date)
                    .ToList();

                // Nhóm các đơn hàng theo ngày
                dataChart = orders
                    .GroupBy(o => o.order_date.Value.Date)
                    .Select(g => new DataForChart
                    {
                        order_date = g.Key.ToString("dd/MM/yyyy"),
                        total_amount = g.Sum(o => o.total_amount)
                    })
                    .ToList();
            }
            else if (typeFilter == 3)
            {
                // Thống kê tháng này
                timeStartForChart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                timeEndStartForChart = timeStartForChart.AddMonths(1).AddTicks(-1);
                orders = db.Orders
                    .Where(x => x.order_date >= timeStartForChart && x.order_date < timeEndStartForChart)
                    .OrderBy(o => o.order_date)
                    .ToList();

                // Nhóm các đơn hàng theo ngày
                dataChart = orders
                    .GroupBy(o => o.order_date.Value.Date)
                    .Select(g => new DataForChart
                    {
                        order_date = g.Key.ToString("dd/MM/yyyy"),
                        total_amount = g.Sum(o => o.total_amount)
                    })
                    .ToList();
            }
            else if (typeFilter == 4)
            {
                // Thống kê theo thời gian tự chọn
                orders = db.Orders
                    .Where(x => x.order_date >= timeStartForChart && x.order_date <= timeEndStartForChart)
                    .OrderBy(o => o.order_date)
                    .ToList();

                // Nhóm các đơn hàng theo ngày
                dataChart = orders
                    .GroupBy(o => o.order_date.Value.Date)
                    .Select(g => new DataForChart
                    {
                        order_date = g.Key.ToString("dd/MM/yyyy"),
                        total_amount = g.Sum(o => o.total_amount)
                    })
                    .ToList();
            }

            // Nếu các đơn hàng đã được lấy trong từng điều kiện thống kê riêng, ta sẽ không cần nhóm lại một lần nữa
            if (orders != null && typeFilter == null)
            {
                // Nhóm các đơn hàng theo ngày và tính tổng số tiền cho mỗi ngày
                var groupedData = orders
                    .GroupBy(o => o.order_date.Value.Date)
                    .Select(g => new DataForChart
                    {
                        order_date = g.Key.ToString("dd/MM/yyyy"),
                        total_amount = g.Sum(o => o.total_amount)
                    })
                    .ToList();

                // Thêm dữ liệu vào dataChart
                dataChart.AddRange(groupedData);
            }

            return dataChart;
        }
    }
}