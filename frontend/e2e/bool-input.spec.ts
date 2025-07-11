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

  // Chekboxes with description visiable tests
  await expect(
    appFrame!.getByTestId('checkbox-true-state-width-description')
  ).toBeVisible();
  await expect(
    appFrame!.getByTestId('checkbox-false-state-width-description')
  ).toBeVisible();
  await expect(
    appFrame!.getByTestId('checkbox-true-state-width-description-disabled')
  ).toBeVisible();
  await expect(
    appFrame!.getByTestId('checkbox-true-state-width-description-invalid')
  ).toBeVisible();
  await expect(
    appFrame!.getByTestId('checkbox-null-state-width-description')
  ).toBeVisible();

  // Chekboxes without description visiable tests
  await expect(
    appFrame!.getByTestId('checkbox-true-state-width')
  ).toBeVisible();
  await expect(
    appFrame!.getByTestId('checkbox-false-state-width')
  ).toBeVisible();
  await expect(
    appFrame!.getByTestId('checkbox-true-state-width-disabled')
  ).toBeVisible();
  await expect(
    appFrame!.getByTestId('checkbox-true-state-width-invalid')
  ).toBeVisible();
  await expect(
    appFrame!.getByTestId('checkbox-null-state-width')
  ).toBeVisible();

  // Switches with description visiable tests
  await expect(
    appFrame!.getByTestId('switch-true-state-width-description')
  ).toBeVisible();
  await expect(
    appFrame!.getByTestId('switch-false-state-width-description')
  ).toBeVisible();
  await expect(
    appFrame!.getByTestId('switch-true-state-width-description-disabled')
  ).toBeVisible();
  await expect(
    appFrame!.getByTestId('switch-true-state-width-description-invalid')
  ).toBeVisible();

  // Switches without description visiable tests
  await expect(appFrame!.getByTestId('switch-true-state-width')).toBeVisible();
  await expect(appFrame!.getByTestId('switch-false-state-width')).toBeVisible();
  await expect(
    appFrame!.getByTestId('switch-true-state-width-disabled')
  ).toBeVisible();
  await expect(
    appFrame!.getByTestId('switch-true-state-width-description-invalid')
  ).toBeVisible();

  // Toggles with description visiable tests
  await expect(
    appFrame!.getByTestId('toggle-true-state-width-description')
  ).toBeVisible();
  await expect(
    appFrame!.getByTestId('toggle-false-state-width-description')
  ).toBeVisible();
  await expect(
    appFrame!.getByTestId('toggle-true-state-width-description-disabled')
  ).toBeVisible();
  await expect(
    appFrame!.getByTestId('toggle-true-state-width-description-invalid')
  ).toBeVisible();

  // Toggles without description visiable tests
  await expect(appFrame!.getByTestId('toggle-true-state-width')).toBeVisible();
  await expect(appFrame!.getByTestId('toggle-false-state-width')).toBeVisible();
  await expect(
    appFrame!.getByTestId('toggle-true-state-width-disabled')
  ).toBeVisible();
  await expect(
    appFrame!.getByTestId('toggle-true-state-width-invalid')
  ).toBeVisible();
});

test('checkbox and related spans visibility after click', async ({ page }) => {
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

  const checkboxButtonTrueDescription = appFrame!.getByTestId(
    'checkbox-true-state-width-description'
  );
  await expect(checkboxButtonTrueDescription).toHaveAttribute(
    'data-state',
    'checked'
  );
  await checkboxButtonTrueDescription.click();
  await expect(checkboxButtonTrueDescription).toHaveAttribute(
    'data-state',
    'unchecked'
  );

  const checkboxButtonFalseDescription = appFrame!.getByTestId(
    'checkbox-false-state-width-description'
  );
  await expect(checkboxButtonFalseDescription).toHaveAttribute(
    'data-state',
    'unchecked'
  );
  await checkboxButtonFalseDescription.click();
  await expect(checkboxButtonFalseDescription).toHaveAttribute(
    'data-state',
    'checked'
  );

  const checkboxButtonTrueDescriptionDisabled = appFrame!.getByTestId(
    'checkbox-true-state-width-description-disabled'
  );
  await expect(checkboxButtonTrueDescriptionDisabled).toHaveAttribute(
    'data-state',
    'checked'
  );
  await checkboxButtonTrueDescriptionDisabled.click();
  await expect(checkboxButtonTrueDescriptionDisabled).toHaveAttribute(
    'data-state',
    'checked'
  );
});
