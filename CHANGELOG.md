# Changelog

All notable changes to **HitDrinkPos** are recorded here. The format follows
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and this project
adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.0] — 2026-06-16

Initial public release of the `HitDrinkPos` drink-pricing core library.

### Added

- Drink base pricing (`Pricing`): per-cup base prices and M/L size surcharge.
- Order checkout (`Receipt`): line totals, subtotal, average-per-cup, and
  full order total with spend-threshold discount applied.
- Spend-threshold discount (`Discount`): NT$10 off at NT$100, NT$30 off at NT$200.
- Topping pricing scaffold (`Toppings`) — actual prices land in a later release.
- Hit branch-strategy exercise scaffolding under `docs/tasks/`.

### Fixed

- `Discount.DiscountFor` now applies the discount when the subtotal lands
  exactly on the tier threshold (was off-by-one with `>` instead of `>=`). (#2)
- `Pricing.cs` build under `ImplicitUsings=disable` — added missing
  `using System;` so `ArgumentException` resolves.

### Notes

- This is a library; no executable artifact is published.
- Build: `dotnet build` · Test: `dotnet test` (13 passing).
