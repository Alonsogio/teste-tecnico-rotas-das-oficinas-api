using FluentValidation;
using RO.DevTest.Application.Features.Sales.Queries.GetSalesReportByPeriodQuery;

namespace RO.DevTest.Application.Validators.Sales
{
  public class GetSalesReportByPeriodQueryValidator : AbstractValidator<GetSalesReportByPeriodQuery>
  {
    public GetSalesReportByPeriodQueryValidator()
    {
      RuleFor(x => x.Inicio)
          .NotEmpty().WithMessage("A data de início é obrigatória");

      RuleFor(x => x.Fim)
          .NotEmpty().WithMessage("A data de fim é obrigatória")
          .GreaterThanOrEqualTo(x => x.Inicio).WithMessage("A data de fim deve ser igual ou posterior à data de início");

      RuleFor(x => x)
          .Must(x => (x.Fim - x.Inicio).TotalDays <= 365)
          .WithMessage("O intervalo de datas não pode ultrapassar 365 dias");
    }
  }
}