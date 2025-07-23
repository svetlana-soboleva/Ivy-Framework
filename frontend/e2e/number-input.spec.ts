import { test, expect, type Page } from '@playwright/test';

// Shared setup function for number input tests
async function setupNumberInputPage(page: Page): Promise<void> {
  await page.goto('/');
  await page.waitForLoadState('networkidle');

  // Navigate to Number Inputs app
  const searchInput = page.getByTestId('sidebar-search');
  await expect(searchInput).toBeVisible();
  await searchInput.click();
  await searchInput.fill('number input');
  await searchInput.press('Enter');

  const firstResult = page
    .locator('[data-sidebar="menu-item"], [data-sidebar="menu-sub-item"]')
    .filter({ hasText: /Number Input/i })
    .first();

  await expect(firstResult).toBeVisible();
  await firstResult.click();
  await page.waitForLoadState('networkidle');
}

test.describe('Number Inputs - Variants Section', () => {
  test.beforeEach(async ({ page }) => {
    await setupNumberInputPage(page);
  });

  test('should display all variants in the grid', async ({ page }) => {
    // Check that the variants section exists
    await expect(
      page.locator('h2').filter({ hasText: 'Variants' })
    ).toBeVisible();

    // Check that the main number input variants are visible
    await expect(page.getByTestId('number-input-nullable-main')).toBeVisible();
    await expect(page.getByTestId('number-input-int-main')).toBeVisible();
    await expect(
      page.getByTestId('number-input-int-disabled-main')
    ).toBeVisible();
    await expect(
      page.getByTestId('number-input-int-invalid-main')
    ).toBeVisible();
    await expect(
      page.getByTestId('number-input-int-arrows-main')
    ).toBeVisible();

    // Check that the slider variants are visible
    await expect(
      page.getByTestId('number-input-nullable-slider-main')
    ).toBeVisible();
    await expect(
      page.getByTestId('number-input-int-slider-main')
    ).toBeVisible();
    await expect(
      page.getByTestId('number-input-int-disabled-slider-main')
    ).toBeVisible();
    await expect(
      page.getByTestId('number-input-int-invalid-slider-main')
    ).toBeVisible();
  });

  test('should test number input interaction', async ({ page }) => {
    const numberInput = page.getByTestId('number-input-int-main');

    // Clear the input first, then test typing a value
    await numberInput.clear();
    await numberInput.fill('42');
    await expect(numberInput).toHaveValue('42');

    // Test clearing the value
    await numberInput.clear();
    await expect(numberInput).toHaveValue('');
  });

  test('should test nullable number input', async ({ page }) => {
    const nullableInput = page.getByTestId('number-input-nullable-main');

    // Clear the input first, then test typing a value
    await nullableInput.clear();
    await nullableInput.fill('123');
    await expect(nullableInput).toHaveValue('123');

    // Test clearing the value (should show clear button)
    await nullableInput.clear();
    await expect(nullableInput).toHaveValue('');
  });

  test('should test disabled number input', async ({ page }) => {
    const disabledInput = page.getByTestId('number-input-int-disabled-main');

    // Test that the input is disabled
    await expect(disabledInput).toBeDisabled();

    // Test that the input has a value but we can't modify it
    await expect(disabledInput).toHaveValue('12,345');
  });

  test('should test slider input interaction', async ({ page }) => {
    const sliderInput = page.getByTestId('number-input-int-slider-main');

    // For slider inputs, we should test that the element exists and is interactive
    await expect(sliderInput).toBeVisible();

    // Test that it's not disabled
    await expect(sliderInput).not.toHaveAttribute('aria-disabled', 'true');
  });

  test('should test arrows input interaction', async ({ page }) => {
    const arrowsInput = page.getByTestId('number-input-int-arrows-main');

    // Clear the input first, then test typing a value
    await arrowsInput.clear();
    await arrowsInput.fill('10');
    await expect(arrowsInput).toHaveValue('10');

    // Test that arrows are visible (the input should have the pr-14 class for right padding)
    await expect(arrowsInput).toHaveClass(/pr-14/);
  });
});
