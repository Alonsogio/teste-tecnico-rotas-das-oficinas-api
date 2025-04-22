using FluentValidation;
using RO.DevTest.Application.Features.Sales.Queries.GetSalesReportByPeriodQuery;

namespace RO.DevTest.Application.Validators.Sales
{
  public class GetSalesReportByPeriodQueryValidator : AbstractValidator<GetSalesReportByPeriodQuery>
  {
    public GetSalesReportByPeriodQueryValidator()
    {
      RuleFor(x => x.Fim)
          .GreaterThanOrEqualTo(x => x.Inicio).WithMessage("A data de fim deve ser igual ou posterior à data de início");

      RuleFor(x => x)
          .Must(x => (x.Fim - x.Inicio).TotalDays <= 365)
          .WithMessage("O intervalo de datas não pode ultrapassar 365 dias");
    }
  }
}