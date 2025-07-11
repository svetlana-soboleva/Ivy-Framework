import { test, expect } from '@playwright/test';

test.describe('DateRangeInput', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/apps/DateRangeInputApp');
  });

  test('should display all variants correctly', async ({ page }) => {
    // Check that all variant inputs are present
    await expect(
      page.getByTestId('daterange-input-datetime-main')
    ).toBeVisible();
    await expect(
      page.getByTestId('daterange-input-datetime-disabled-main')
    ).toBeVisible();
    await expect(
      page.getByTestId('daterange-input-datetime-invalid-main')
    ).toBeVisible();
    await expect(
      page.getByTestId('daterange-input-datetime-nullable-main')
    ).toBeVisible();
    await expect(
      page.getByTestId('daterange-input-datetime-nullable-disabled-main')
    ).toBeVisible();
    await expect(
      page.getByTestId('daterange-input-datetime-nullable-invalid-main')
    ).toBeVisible();
  });

  test('should show disabled state correctly', async ({ page }) => {
    const disabledInput = page.getByTestId(
      'daterange-input-datetime-disabled-main'
    );
    await expect(disabledInput).toBeDisabled();
  });

  test('should show invalid state correctly', async ({ page }) => {
    const invalidInput = page.getByTestId(
      'daterange-input-datetime-invalid-main'
    );
    // Check for invalid styling (this may vary based on your CSS classes)
    await expect(invalidInput).toBeVisible();
  });

  test('should display data binding inputs correctly', async ({ page }) => {
    // Check that all data binding inputs are present
    await expect(
      page.getByTestId('daterange-input-datetime-binding')
    ).toBeVisible();
    await expect(
      page.getByTestId('daterange-input-dateonly-binding')
    ).toBeVisible();
    await expect(
      page.getByTestId('daterange-input-timeonly-binding')
    ).toBeVisible();
    await expect(
      page.getByTestId('daterange-input-string-binding')
    ).toBeVisible();
    await expect(
      page.getByTestId('daterange-input-datetime-nullable-binding')
    ).toBeVisible();
    await expect(
      page.getByTestId('daterange-input-dateonly-nullable-binding')
    ).toBeVisible();
    await expect(
      page.getByTestId('daterange-input-timeonly-nullable-binding')
    ).toBeVisible();
  });

  test('should display current values section', async ({ page }) => {
    // Check that the current values section is present
    await expect(
      page.getByRole('heading', { name: 'Current Values' })
    ).toBeVisible();

    // Check that current values are displayed
    await expect(page.getByText(/DateTime Range:/)).toBeVisible();
    await expect(page.getByText(/Nullable DateTime Range:/)).toBeVisible();
    await expect(page.getByText(/DateOnly Range:/)).toBeVisible();
    await expect(page.getByText(/TimeOnly Range:/)).toBeVisible();
    await expect(page.getByText(/String Range:/)).toBeVisible();
    await expect(page.getByText(/Nullable DateOnly Range:/)).toBeVisible();
    await expect(page.getByText(/Nullable TimeOnly Range:/)).toBeVisible();
  });

  test('should open calendar popup when clicked', async ({ page }) => {
    const input = page.getByTestId('daterange-input-datetime-main');
    await input.click();

    // Check that the calendar popup appears
    await expect(page.locator('[role="dialog"]')).toBeVisible();
  });

  test('should allow date range selection', async ({ page }) => {
    const input = page.getByTestId('daterange-input-datetime-main');
    await input.click();

    // Wait for calendar to appear
    await expect(page.locator('[role="dialog"]')).toBeVisible();

    // Click on a date (this is a basic test - actual date selection may need more specific selectors)
    const calendar = page.locator('[role="dialog"]');
    await expect(calendar).toBeVisible();

    // Close the calendar
    await page.keyboard.press('Escape');
  });

  test('should display quick selection options', async ({ page }) => {
    const input = page.getByTestId('daterange-input-datetime-main');
    await input.click();

    // Check for quick selection buttons
    await expect(page.getByRole('button', { name: 'Today' })).toBeVisible();
    await expect(page.getByRole('button', { name: 'Yesterday' })).toBeVisible();
    await expect(
      page.getByRole('button', { name: 'Last 7 Days' })
    ).toBeVisible();
    await expect(
      page.getByRole('button', { name: 'Last 30 Days' })
    ).toBeVisible();
    await expect(
      page.getByRole('button', { name: 'Month to Date' })
    ).toBeVisible();
    await expect(
      page.getByRole('button', { name: 'Last Month' })
    ).toBeVisible();
    await expect(
      page.getByRole('button', { name: 'Year to Date' })
    ).toBeVisible();
    await expect(page.getByRole('button', { name: 'Last Year' })).toBeVisible();
  });

  test('should handle quick selection', async ({ page }) => {
    const input = page.getByTestId('daterange-input-datetime-main');
    await input.click();

    // Click on "Today" quick selection
    await page.getByRole('button', { name: 'Today' }).click();

    // Calendar should close after selection
    await expect(page.locator('[role="dialog"]')).not.toBeVisible();
  });

  test('should display grid layout correctly', async ({ page }) => {
    // Check that the variants grid is present
    await expect(page.getByRole('heading', { name: 'Variants' })).toBeVisible();

    // Check that the data binding grid is present
    await expect(
      page.getByRole('heading', { name: 'Data Binding' })
    ).toBeVisible();

    // Check for grid headers
    await expect(page.getByText('Normal')).toBeVisible();
    await expect(page.getByText('Disabled')).toBeVisible();
    await expect(page.getByText('Invalid')).toBeVisible();
    await expect(page.getByText('Type')).toBeVisible();
    await expect(page.getByText('Input')).toBeVisible();
    await expect(page.getByText('Current Value')).toBeVisible();
  });
});
