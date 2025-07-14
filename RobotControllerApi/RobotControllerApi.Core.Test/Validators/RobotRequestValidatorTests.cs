using FluentValidation.TestHelper;
using RobotControllerApi.Core.Entities;
using RobotControllerApi.Core.Models;
using RobotControllerApi.Core.Validators;
using Xunit;

namespace RobotControllerApi.Core.Test.Validators
{
    public class RobotRequestValidatorTests
    {
        private readonly RobotRequestValidator _validator;

        public RobotRequestValidatorTests()
        {
            _validator = new RobotRequestValidator();
        }

        private RobotRequest CreateValidRequest()
        {
            return new RobotRequest
            {
                Name = "TestBot",
                RoomId = 1,
                X = 1,
                Y = 1,
                Facing = Direction.N,
                Commands = "FLR",
                Room = new Room
                {
                    Width = 5,
                    Height = 5
                }
            };
        }

        [Fact]
        public void Should_Have_Error_When_Commands_Are_Empty()
        {
            var model = CreateValidRequest();
            model.Commands = "";

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Commands);
        }

        [Fact]
        public void Should_Have_Error_When_Commands_Contain_Invalid_Characters()
        {
            var model = CreateValidRequest();
            model.Commands = "FXYZ";

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Commands);
        }

        [Fact]
        public void Should_Have_Error_When_X_Is_Negative()
        {
            var model = CreateValidRequest();
            model.X = -1;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.X);
        }

        [Fact]
        public void Should_Have_Error_When_Y_Is_Negative()
        {
            var model = CreateValidRequest();
            model.Y = -1;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Y);
        }

        [Fact]
        public void Should_Have_Error_When_Room_Is_Null()
        {
            var model = CreateValidRequest();
            model.Room = null;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Room);
        }

        [Fact]
        public void Should_Have_Error_When_Room_Width_Is_Less_Than_Or_Equal_To_Zero()
        {
            var model = CreateValidRequest();
            model.Room.Width = 0;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor("Room.Width");
        }

        [Fact]
        public void Should_Have_Error_When_Room_Height_Is_Less_Than_Or_Equal_To_Zero()
        {
            var model = CreateValidRequest();
            model.Room.Height = -1;

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor("Room.Height");
        }

        [Fact]
        public void Should_Not_Have_Error_For_Valid_Request()
        {
            var model = CreateValidRequest();

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}