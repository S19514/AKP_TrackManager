using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AKP_TrackManager.Migrations
{
    public partial class AKP_TrackManager_Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accident",
                columns: table => new
                {
                    AccidentId = table.Column<int>(type: "int", nullable: false),
                    AccidentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    AnyoneInjured = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accident", x => x.AccidentId);
                });

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    CarId = table.Column<int>(type: "int", nullable: false),
                    Make = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    EngingeCapacity = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    EnginePower = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.CarId);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Town = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    Street = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Country = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<int>(type: "int", nullable: false),
                    Surname = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    EmailAddress = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    IsAscendant = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.MemberId);
                });

            migrationBuilder.CreateTable(
                name: "TrackConfiguration",
                columns: table => new
                {
                    TrackId = table.Column<int>(type: "int", nullable: false),
                    Reversable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("TrackConfiguration_pk", x => x.TrackId);
                });

            migrationBuilder.CreateTable(
                name: "CarAccidentByMember",
                columns: table => new
                {
                    CarAccidentMemberId = table.Column<int>(type: "int", nullable: false),
                    Member_MemberId = table.Column<int>(type: "int", nullable: false),
                    Car_CarId = table.Column<int>(type: "int", nullable: false),
                    Accident_AccidentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CarAccidentByMember_pk", x => x.CarAccidentMemberId);
                    table.ForeignKey(
                        name: "CarAccidentByMember_Accident",
                        column: x => x.Accident_AccidentId,
                        principalTable: "Accident",
                        principalColumn: "AccidentId");
                    table.ForeignKey(
                        name: "CarAccidentByMember_Car",
                        column: x => x.Car_CarId,
                        principalTable: "Car",
                        principalColumn: "CarId");
                    table.ForeignKey(
                        name: "CarAccidentByMember_Member",
                        column: x => x.Member_MemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                });

            migrationBuilder.CreateTable(
                name: "CarMember",
                columns: table => new
                {
                    CarMemberId = table.Column<int>(type: "int", nullable: false),
                    Member_MemberId = table.Column<int>(type: "int", nullable: false),
                    Car_CarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarMember", x => x.CarMemberId);
                    table.ForeignKey(
                        name: "CarDriver_Car",
                        column: x => x.Car_CarId,
                        principalTable: "Car",
                        principalColumn: "CarId");
                    table.ForeignKey(
                        name: "CarDriver_Member",
                        column: x => x.Member_MemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                });

            migrationBuilder.CreateTable(
                name: "ClubMembership",
                columns: table => new
                {
                    MembershipId = table.Column<int>(type: "int", nullable: false),
                    JoinDate = table.Column<int>(type: "int", nullable: false),
                    FeeAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Member_MemberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ClubMembership_pk", x => x.MembershipId);
                    table.ForeignKey(
                        name: "ClubMembership_Member",
                        column: x => x.Member_MemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                });

            migrationBuilder.CreateTable(
                name: "Training",
                columns: table => new
                {
                    TrainingId = table.Column<int>(type: "int", nullable: false),
                    TrackConfiguration_TrackId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time(0)", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time(0)", nullable: false),
                    Location_LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Training", x => x.TrainingId);
                    table.ForeignKey(
                        name: "Training_Location",
                        column: x => x.Location_LocationId,
                        principalTable: "Location",
                        principalColumn: "LocationId");
                    table.ForeignKey(
                        name: "Training_TrackConfiguration",
                        column: x => x.TrackConfiguration_TrackId,
                        principalTable: "TrackConfiguration",
                        principalColumn: "TrackId");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false),
                    ClubMembership_MembershipId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Member_MemberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "Payment_ClubMembership",
                        column: x => x.ClubMembership_MembershipId,
                        principalTable: "ClubMembership",
                        principalColumn: "MembershipId");
                    table.ForeignKey(
                        name: "Payment_Member",
                        column: x => x.Member_MemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                });

            migrationBuilder.CreateTable(
                name: "Lap",
                columns: table => new
                {
                    LapId = table.Column<int>(type: "int", nullable: false),
                    MeasuredTime = table.Column<int>(type: "int", nullable: false),
                    PenaltyTime = table.Column<int>(type: "int", nullable: false),
                    AbsoluteTime = table.Column<int>(type: "int", nullable: false),
                    Training_TrainingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lap", x => x.LapId);
                    table.ForeignKey(
                        name: "Lap_Training",
                        column: x => x.Training_TrainingId,
                        principalTable: "Training",
                        principalColumn: "TrainingId");
                });

            migrationBuilder.CreateTable(
                name: "TrainingAttandance",
                columns: table => new
                {
                    TrainingAttandanceId = table.Column<int>(type: "int", nullable: false),
                    Training_TrainingId = table.Column<int>(type: "int", nullable: false),
                    Member_MemberId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingAttandance", x => x.TrainingAttandanceId);
                    table.ForeignKey(
                        name: "TrainingAttandance_Member",
                        column: x => x.Member_MemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                    table.ForeignKey(
                        name: "TrainingAttandance_Training",
                        column: x => x.Training_TrainingId,
                        principalTable: "Training",
                        principalColumn: "TrainingId");
                });

            migrationBuilder.CreateTable(
                name: "MemberCarOnLap",
                columns: table => new
                {
                    MemberLapId = table.Column<int>(type: "int", nullable: false),
                    Member_MemberId = table.Column<int>(type: "int", nullable: false),
                    Car_CarId = table.Column<int>(type: "int", nullable: false),
                    Lap_LapId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("MemberCarOnLap_pk", x => x.MemberLapId);
                    table.ForeignKey(
                        name: "MemberCarOnLap_Lap",
                        column: x => x.Lap_LapId,
                        principalTable: "Lap",
                        principalColumn: "LapId");
                    table.ForeignKey(
                        name: "MemberOnLap_Car",
                        column: x => x.Car_CarId,
                        principalTable: "Car",
                        principalColumn: "CarId");
                    table.ForeignKey(
                        name: "MemberOnLap_Member",
                        column: x => x.Member_MemberId,
                        principalTable: "Member",
                        principalColumn: "MemberId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarAccidentByMember_Accident_AccidentId",
                table: "CarAccidentByMember",
                column: "Accident_AccidentId");

            migrationBuilder.CreateIndex(
                name: "IX_CarAccidentByMember_Car_CarId",
                table: "CarAccidentByMember",
                column: "Car_CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarAccidentByMember_Member_MemberId",
                table: "CarAccidentByMember",
                column: "Member_MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_CarMember_Car_CarId",
                table: "CarMember",
                column: "Car_CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarMember_Member_MemberId",
                table: "CarMember",
                column: "Member_MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubMembership_Member_MemberId",
                table: "ClubMembership",
                column: "Member_MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Lap_Training_TrainingId",
                table: "Lap",
                column: "Training_TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberCarOnLap_Car_CarId",
                table: "MemberCarOnLap",
                column: "Car_CarId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberCarOnLap_Lap_LapId",
                table: "MemberCarOnLap",
                column: "Lap_LapId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberCarOnLap_Member_MemberId",
                table: "MemberCarOnLap",
                column: "Member_MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ClubMembership_MembershipId",
                table: "Payment",
                column: "ClubMembership_MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_Member_MemberId",
                table: "Payment",
                column: "Member_MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Training_Location_LocationId",
                table: "Training",
                column: "Location_LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Training_TrackConfiguration_TrackId",
                table: "Training",
                column: "TrackConfiguration_TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingAttandance_Member_MemberId",
                table: "TrainingAttandance",
                column: "Member_MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingAttandance_Training_TrainingId",
                table: "TrainingAttandance",
                column: "Training_TrainingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarAccidentByMember");

            migrationBuilder.DropTable(
                name: "CarMember");

            migrationBuilder.DropTable(
                name: "MemberCarOnLap");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "TrainingAttandance");

            migrationBuilder.DropTable(
                name: "Accident");

            migrationBuilder.DropTable(
                name: "Lap");

            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "ClubMembership");

            migrationBuilder.DropTable(
                name: "Training");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "TrackConfiguration");
        }
    }
}
