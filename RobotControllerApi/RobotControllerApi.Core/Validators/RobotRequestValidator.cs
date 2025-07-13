using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using RobotControllerApi.Core.Models;

namespace RobotControllerApi.Core.Validators
{
   public class RobotRequestValidator : AbstractValidator<RobotRequest>
    {
        public RobotRequestValidator()
        {
            RuleFor(x => x.Commands)
                .NotEmpty().WithMessage("Commands cannot be empty.")
                .Matches("^[LRF]+$").WithMessage("Commands must only contain L, R, or F.");

            RuleFor(x => x.X).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Y).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Facing).IsInEnum();

            RuleFor(x => x.Room).NotNull().WithMessage("Room is required.");

            When(x => x.Room != null, () =>
            {
                RuleFor(x => x.Room.Width).GreaterThan(0);
                RuleFor(x => x.Room.Height).GreaterThan(0);
            });
        }
    }
}
