import { test, expect } from '@playwright/test';

test('debug sidebar search', async ({ page }) => {
  await page.goto('/app');
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

  await expect(firstResult).toBeVisible();
  await firstResult.click();

  // Wait for the page to load
  await page.waitForLoadState('networkidle');

  // Check that we can see the main content
  await expect(page.locator('body')).toBeVisible();

  // Look for h1 saying "Bool Input"
  const appFrameElement = await page.waitForSelector(
    'iframe[src*="bool-input"]'
  );
  const appFrame = await appFrameElement.contentFrame();
  expect(appFrame).not.toBeNull();

  // Now look for the h1 in the app frame
  const h1 = appFrame!.locator('h1');
  await expect(h1).toContainText('BoolInput');

  // Now look for the h2 in the app frame
  const h2 = appFrame!.locator('h2');
  await expect(h2.nth(0)).toContainText('Variants');

  await expect(
    appFrame!.locator('code', { hasText: 'BoolInputs.Checkbox' })
  ).toBeVisible();

  await expect(
    appFrame!.locator('code', { hasText: 'BoolInputs.Switch' })
  ).toBeVisible();

  await expect(
    appFrame!.locator('code', { hasText: 'BoolInputs.Toggle' })
  ).toBeVisible();

  const checkboxButton = appFrame!.locator('#glju9s8ied');
  const checkedSpan = checkboxButton.locator('span[data-state="checked"]');
  const checkBoxButtonLower = appFrame!.locator('#q2fgjba5yb');
  const checkedSpanButtonLower = checkBoxButtonLower.locator(
    'span[data-state="checked"]'
  );
  const disabledCheckboxButton = appFrame!.locator('#lnax7xp7yk');
  const checkedSpanDisabledCheckboxButton = disabledCheckboxButton.locator(
    'span[data-state="checked"]'
  );
  const disabledCheckboxButtonLower = appFrame!.locator('#b6qatvj0yf');
  const checkedSpanDisabledCheckboxButtonLower =
    disabledCheckboxButtonLower.locator('span[data-state="checked"]');
  const invalidCheckboxButton = appFrame!.locator('#udbffh201n');
  const checkedSpanInvalidCheckboxButton = invalidCheckboxButton.locator(
    'span[data-state="checked"]'
  );
  const invalidCheckboxButtonLower = appFrame!.locator('#m05keqhbfb');
  const checkedSpanInvalidCheckboxButtonLower =
    invalidCheckboxButtonLower.locator('span[data-state="checked"]');

  await checkboxButton.click();
  await expect(checkedSpan).toBeHidden();
  await expect(checkedSpanButtonLower).toBeHidden();
  await expect(checkedSpanDisabledCheckboxButton).toBeHidden();
  await expect(checkedSpanDisabledCheckboxButtonLower).toBeHidden();
  await expect(checkedSpanInvalidCheckboxButton).toBeHidden();
  await expect(checkedSpanInvalidCheckboxButtonLower).toBeHidden();
});
