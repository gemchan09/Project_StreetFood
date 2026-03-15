using Microsoft.EntityFrameworkCore;
using TouristGuide.Shared.Models;

namespace TouristGuide.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PointOfInterest> PointsOfInterest => Set<PointOfInterest>();
    public DbSet<Tour> Tours => Set<Tour>();
    public DbSet<NarrationLog> NarrationLogs => Set<NarrationLog>();
    public DbSet<LocationLog> LocationLogs => Set<LocationLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PointOfInterest>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Name).HasMaxLength(200);
            e.Property(p => p.Category).HasMaxLength(50);
        });

        modelBuilder.Entity<Tour>(e =>
        {
            e.HasKey(t => t.Id);
            e.HasMany(t => t.Pois).WithOne().HasForeignKey(p => p.TourId);
        });

        modelBuilder.Entity<NarrationLog>(e =>
        {
            e.HasKey(n => n.Id);
        });

        modelBuilder.Entity<LocationLog>(e =>
        {
            e.HasKey(l => l.Id);
        });

        // Seed data - TP.HCM Quận 1
        modelBuilder.Entity<Tour>().HasData(new Tour
        {
            Id = 1,
            Name = "Tour Quận 1 - TP.HCM",
            NameEn = "District 1 - HCMC Tour",
            Description = "Khám phá các địa điểm lịch sử nổi tiếng tại Quận 1",
            DescriptionEn = "Explore famous historical landmarks in District 1",
            IsActive = true,
            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        });

        modelBuilder.Entity<PointOfInterest>().HasData(
            new PointOfInterest
            {
                Id = 1, Name = "Nhà thờ Đức Bà Sài Gòn", NameEn = "Notre-Dame Cathedral Basilica of Saigon",
                Description = "Nhà thờ Đức Bà Sài Gòn, tên chính thức là Vương cung thánh đường Chính tòa Đức Mẹ Vô nhiễm Nguyên tội, là nhà thờ chính tòa của Tổng giáo phận Thành phố Hồ Chí Minh, được xây dựng từ 1863-1880 theo kiến trúc Roman-Gothic.",
                DescriptionEn = "Notre-Dame Cathedral Basilica of Saigon is the cathedral of the Archdiocese of Ho Chi Minh City, built between 1863-1880 in Romanesque and Gothic styles.",
                Latitude = 10.7798, Longitude = 106.6990, TriggerRadiusMeters = 80, Priority = 10,
                NarrationText = "Chào mừng bạn đến với Nhà thờ Đức Bà Sài Gòn! Đây là một trong những công trình kiến trúc tiêu biểu của thành phố, được người Pháp xây dựng từ năm 1863 đến 1880. Nhà thờ mang phong cách kiến trúc Roman kết hợp Gothic, với hai tháp chuông cao 58 mét. Toàn bộ vật liệu xây dựng đều được nhập từ Pháp. Bức tượng Đức Mẹ Hòa Bình được đặt tại quảng trường phía trước vào năm 1959.",
                NarrationTextEn = "Welcome to Notre-Dame Cathedral Basilica of Saigon! This is one of the most iconic architectural landmarks of the city, built by the French between 1863 and 1880. The cathedral features Romanesque and Gothic architectural styles, with two bell towers standing 58 meters tall. All construction materials were imported from France. The statue of the Virgin Mary was placed in the square in front in 1959.",
                Category = "religious", IsActive = true, TourId = 1,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new PointOfInterest
            {
                Id = 2, Name = "Bưu điện Trung tâm Sài Gòn", NameEn = "Saigon Central Post Office",
                Description = "Bưu điện Trung tâm Sài Gòn là một trong những công trình kiến trúc tiêu biểu tại TP.HCM, được xây dựng trong khoảng 1886-1891 do kiến trúc sư Gustave Eiffel thiết kế.",
                DescriptionEn = "Saigon Central Post Office is one of the most iconic buildings in HCMC, built between 1886-1891, designed by architect Gustave Eiffel.",
                Latitude = 10.7800, Longitude = 106.6998, TriggerRadiusMeters = 60, Priority = 9,
                NarrationText = "Đây là Bưu điện Trung tâm Sài Gòn, được xây dựng từ năm 1886 đến 1891 theo phong cách kiến trúc thuộc địa Pháp. Công trình được cho là do kiến trúc sư nổi tiếng Gustave Eiffel thiết kế. Bên trong, bạn sẽ thấy hai bản đồ lớn vẽ trên tường từ thời Pháp thuộc và bức chân dung Chủ tịch Hồ Chí Minh.",
                NarrationTextEn = "This is the Saigon Central Post Office, built between 1886 and 1891 in French colonial architectural style. The building is believed to have been designed by the famous architect Gustave Eiffel. Inside, you will find two large maps painted on the walls from the French colonial period and a portrait of President Ho Chi Minh.",
                Category = "historical", IsActive = true, TourId = 1,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new PointOfInterest
            {
                Id = 3, Name = "Dinh Độc Lập", NameEn = "Independence Palace",
                Description = "Dinh Độc Lập (hay Dinh Thống Nhất) là một công trình kiến trúc nổi tiếng tại TP.HCM, nguyên là dinh tổng thống của Việt Nam Cộng hòa.",
                DescriptionEn = "Independence Palace (or Reunification Palace) is a famous landmark in HCMC, formerly the presidential palace of South Vietnam.",
                Latitude = 10.7769, Longitude = 106.6955, TriggerRadiusMeters = 100, Priority = 10,
                NarrationText = "Chào mừng bạn đến Dinh Độc Lập! Đây là nơi chứng kiến khoảnh khắc lịch sử ngày 30 tháng 4 năm 1975 khi xe tăng quân Giải phóng húc đổ cổng chính, đánh dấu sự thống nhất đất nước. Dinh được kiến trúc sư Ngô Viết Thụ thiết kế và xây dựng từ 1962 đến 1966. Tòa nhà có 100 phòng, bao gồm phòng khánh tiết, phòng họp nội các, và hầm chỉ huy dưới lòng đất.",
                NarrationTextEn = "Welcome to Independence Palace! This is where the historic moment of April 30, 1975 took place when the Liberation Army's tank crashed through the main gate, marking the reunification of Vietnam. The palace was designed by architect Ngo Viet Thu and built between 1962 and 1966. The building has 100 rooms, including ceremonial halls, cabinet meeting rooms, and underground command bunkers.",
                Category = "historical", IsActive = true, TourId = 1,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new PointOfInterest
            {
                Id = 4, Name = "Nhà hát Thành phố", NameEn = "Saigon Opera House",
                Description = "Nhà hát Thành phố (hay Nhà hát lớn) là một nhà hát opera tại TP.HCM, được xây dựng vào năm 1897 theo phong cách kiến trúc Flamboyant.",
                DescriptionEn = "The Municipal Theatre of Ho Chi Minh City (Saigon Opera House) was built in 1897 in Flamboyant style architecture.",
                Latitude = 10.7764, Longitude = 106.7031, TriggerRadiusMeters = 60, Priority = 7,
                NarrationText = "Nhà hát Thành phố được xây dựng năm 1897 theo phong cách kiến trúc Flamboyant của Pháp. Đây từng là trụ sở Quốc hội của Việt Nam Cộng hòa. Ngày nay, nhà hát là địa điểm tổ chức các buổi biểu diễn nghệ thuật đẳng cấp quốc tế.",
                NarrationTextEn = "The Municipal Theatre was built in 1897 in French Flamboyant architectural style. It once served as the seat of the National Assembly of South Vietnam. Today, the theater hosts world-class artistic performances.",
                Category = "cultural", IsActive = true, TourId = 1,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new PointOfInterest
            {
                Id = 5, Name = "Chợ Bến Thành", NameEn = "Ben Thanh Market",
                Description = "Chợ Bến Thành là ngôi chợ lớn nhất và nổi tiếng nhất tại trung tâm Quận 1, TP.HCM, được xây dựng từ đầu thế kỷ 20.",
                DescriptionEn = "Ben Thanh Market is the largest and most famous market in the center of District 1, HCMC, built in the early 20th century.",
                Latitude = 10.7725, Longitude = 106.6980, TriggerRadiusMeters = 80, Priority = 8,
                NarrationText = "Chào mừng bạn đến Chợ Bến Thành! Đây là biểu tượng không chính thức của Thành phố Hồ Chí Minh. Chợ được xây dựng từ năm 1912 đến 1914. Tháp đồng hồ phía trước là hình ảnh quen thuộc nhất. Trong chợ có hơn 3000 gian hàng, bán đủ loại từ thực phẩm, quần áo đến đồ lưu niệm. Buổi tối, chợ đêm bên ngoài sẽ mở cửa với nhiều quán ăn đường phố hấp dẫn.",
                NarrationTextEn = "Welcome to Ben Thanh Market! This is the unofficial symbol of Ho Chi Minh City. The market was built between 1912 and 1914. The clock tower in front is its most recognizable feature. Inside, there are over 3000 stalls selling everything from food and clothing to souvenirs. In the evening, the night market opens outside with many attractive street food vendors.",
                Category = "market", IsActive = true, TourId = 1,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new PointOfInterest
            {
                Id = 6, Name = "Phố đi bộ Nguyễn Huệ", NameEn = "Nguyen Hue Walking Street",
                Description = "Phố đi bộ Nguyễn Huệ là đại lộ trung tâm của TP.HCM, được cải tạo thành phố đi bộ vào năm 2015.",
                DescriptionEn = "Nguyen Hue Walking Street is the central boulevard of HCMC, renovated into a pedestrian street in 2015.",
                Latitude = 10.7740, Longitude = 106.7030, TriggerRadiusMeters = 70, Priority = 6,
                NarrationText = "Đây là Phố đi bộ Nguyễn Huệ, được khánh thành năm 2015. Con đường dài 670 mét này từng là kênh đào thời Pháp thuộc, sau đó được lấp thành đại lộ. Quảng trường có tượng đài Chủ tịch Hồ Chí Minh và nhiều tòa nhà lịch sử như khách sạn Rex, trụ sở UBND Thành phố.",
                NarrationTextEn = "This is Nguyen Hue Walking Street, inaugurated in 2015. This 670-meter-long road was once a canal during the French colonial period, later filled in to become a boulevard. The square features a statue of President Ho Chi Minh and many historical buildings such as the Rex Hotel and the City People's Committee headquarters.",
                Category = "street", IsActive = true, TourId = 1,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
