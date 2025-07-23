import { test, expect, type Page } from '@playwright/test';

// Shared setup function
async function setupBoolInputPage(page: Page): Promise<void> {
  await page.goto('/');
  await page.waitForLoadState('networkidle');

  // Find the sidebar search input
  const searchInput = page.getByTestId('sidebar-search');
  await expect(searchInput).toBeVisible();

  // Click the search input
  await searchInput.click();
  // Type 'bool'
  await searchInput.fill('bool');
  // Press Enter
  await searchInput.press('Enter');

  const firstResult = page
    .locator('[data-sidebar="menu-item"], [data-sidebar="menu-sub-item"]')
    .filter({ hasText: /Bool Input/i })
    .first();
  await firstResult.click();

  // Wait for navigation
  await page.waitForLoadState('networkidle');
}

// Shared verification function for all elements
async function verifyAllElements(page: Page) {
  // Look for the h1 in the page
  const h1 = page.locator('h1');
  await expect(h1).toContainText('BoolInput');

  // Look for the h2 in the page
  const h2 = page.locator('h2');
  await expect(h2.nth(0)).toContainText('Variants');

  await expect(
    page.locator('code:has-text("BoolInputs.Checkbox")')
  ).toBeVisible();
  await expect(
    page.locator('code:has-text("BoolInputs.Switch")')
  ).toBeVisible();
  await expect(
    page.locator('code:has-text("BoolInputs.Toggle")')
  ).toBeVisible();

  // Define test IDs for each category - only the ones that actually exist
  const checkboxWithDescriptionIds = [
    'checkbox-true-state-width-description',
    'checkbox-false-state-width-description',
    'checkbox-true-state-width-description-disabled',
    'checkbox-true-state-width-description-invalid',
    'checkbox-null-state-width-description',
  ];

  const checkboxWithoutDescriptionIds = [
    'checkbox-true-state-width',
    'checkbox-false-state-width',
    'checkbox-true-state-width-disabled',
    'checkbox-true-state-width-invalid',
    'checkbox-null-state-width',
  ];

  const switchWithDescriptionIds = [
    'switch-true-state-width-description',
    'switch-false-state-width-description',
    'switch-true-state-width-description-disabled',
    'switch-true-state-width-description-invalid',
    // Note: switch-null-state-width-description doesn't exist - shows "N/A"
  ];

  const switchWithoutDescriptionIds = [
    'switch-true-state-width',
    'switch-false-state-width',
    'switch-true-state-width-disabled',
    'switch-true-state-width-invalid',
    // Note: switch-null-state-width doesn't exist - shows "N/A"
  ];

  const toggleWithDescriptionIds = [
    'toggle-true-state-width-description',
    'toggle-false-state-width-description',
    'toggle-true-state-width-description-disabled',
    'toggle-true-state-width-description-invalid',
    // Note: toggle-null-state-width-description doesn't exist - shows "N/A"
  ];

  const toggleWithoutDescriptionIds = [
    'toggle-true-state-width',
    'toggle-false-state-width',
    'toggle-true-state-width-disabled',
    'toggle-true-state-width-invalid',
    // Note: toggle-null-state-width doesn't exist - shows "N/A"
  ];

  // Verify all checkbox elements with description
  for (const testId of checkboxWithDescriptionIds) {
    await expect(page.getByTestId(testId)).toBeVisible();
  }

  // Verify all checkbox elements without description
  for (const testId of checkboxWithoutDescriptionIds) {
    await expect(page.getByTestId(testId)).toBeVisible();
  }

  // Verify all switch elements with description
  for (const testId of switchWithDescriptionIds) {
    await expect(page.getByTestId(testId)).toBeVisible();
  }

  // Verify all switch elements without description
  for (const testId of switchWithoutDescriptionIds) {
    await expect(page.getByTestId(testId)).toBeVisible();
  }

  // Verify all toggle elements with description
  for (const testId of toggleWithDescriptionIds) {
    await expect(page.getByTestId(testId)).toBeVisible();
  }

  // Verify all toggle elements without description
  for (const testId of toggleWithoutDescriptionIds) {
    await expect(page.getByTestId(testId)).toBeVisible();
  }
}

test.describe('Bool Input Tests', () => {
  test.beforeEach(async ({ page }) => {
    await setupBoolInputPage(page);
  });

  // Helper function to test toggle behavior
  async function testToggleBehavior(
    page: Page,
    testId: string,
    attributeName: 'aria-checked' | 'aria-pressed',
    initialState: string,
    expectedAfterClick: string
  ) {
    const element = page.getByTestId(testId);

    await expect(element).toHaveAttribute(attributeName, initialState);
    await element.click();
    await expect(element).toHaveAttribute(attributeName, expectedAfterClick);
    await element.click();
    await expect(element).toHaveAttribute(attributeName, initialState);
  }

  // Helper function to test null state cycling (mixed -> true -> false -> mixed)
  async function testNullStateCycling(page: Page, testId: string) {
    const element = page.getByTestId(testId);

    await expect(element).toHaveAttribute('aria-checked', 'mixed');
    await element.click();
    await expect(element).toHaveAttribute('aria-checked', 'true');
    await element.click();
    await expect(element).toHaveAttribute('aria-checked', 'false');
    await element.click();
    await expect(element).toHaveAttribute('aria-checked', 'mixed');
  }

  // Helper function to determine attribute based on test ID prefix
  function getAttributeForTestId(
    testId: string
  ): 'aria-checked' | 'aria-pressed' {
    if (testId.startsWith('toggle-')) {
      return 'aria-pressed';
    }
    return 'aria-checked'; // checkbox and switch both use aria-checked
  }

  // Simplified helper functions that auto-determine the attribute
  async function testTrueFalseToggle(page: Page, testId: string) {
    const attribute = getAttributeForTestId(testId);
    await testToggleBehavior(page, testId, attribute, 'true', 'false');
  }

  async function testFalseTrueToggle(page: Page, testId: string) {
    const attribute = getAttributeForTestId(testId);
    await testToggleBehavior(page, testId, attribute, 'false', 'true');
  }

  async function testDisabledState(page: Page, testId: string) {
    const element = page.getByTestId(testId);
    await expect(element).toBeDisabled();
  }

  test('should display all variants and data binding inputs', async ({
    page,
  }) => {
    await verifyAllElements(page);
  });

  test('should test checkbox true/false toggle behavior', async ({ page }) => {
    await testTrueFalseToggle(page, 'checkbox-true-state-width');
  });

  test('should test checkbox false/true toggle behavior', async ({ page }) => {
    await testFalseTrueToggle(page, 'checkbox-false-state-width');
  });

  test('should test checkbox null state cycling', async ({ page }) => {
    await testNullStateCycling(page, 'checkbox-null-state-width');
  });

  test('should test switch true/false toggle behavior', async ({ page }) => {
    await testTrueFalseToggle(page, 'switch-true-state-width');
  });

  test('should test switch false/true toggle behavior', async ({ page }) => {
    await testFalseTrueToggle(page, 'switch-false-state-width');
  });

  test('should test toggle true/false toggle behavior', async ({ page }) => {
    await testTrueFalseToggle(page, 'toggle-true-state-width');
  });

  test('should test toggle false/true toggle behavior', async ({ page }) => {
    await testFalseTrueToggle(page, 'toggle-false-state-width');
  });

  test('should verify disabled states', async ({ page }) => {
    await testDisabledState(page, 'checkbox-true-state-width-disabled');
    await testDisabledState(page, 'switch-true-state-width-disabled');
    await testDisabledState(page, 'toggle-true-state-width-disabled');
  });
});
