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

  // Checkbox true state with description should be unchecked after click
  const checkboxButtonTrueDescription = appFrame!.getByTestId(
    'checkbox-true-state-width-description'
  );
  await expect(checkboxButtonTrueDescription).toHaveAttribute(
    'aria-checked',
    'true'
  );
  await checkboxButtonTrueDescription.click();
  await expect(checkboxButtonTrueDescription).toHaveAttribute(
    'aria-checked',
    'false'
  );
  await checkboxButtonTrueDescription.click();

  // Checkbox false state with description should be ckecked after click
  const checkboxButtonFalseDescription = appFrame!.getByTestId(
    'checkbox-false-state-width-description'
  );
  await expect(checkboxButtonFalseDescription).toHaveAttribute(
    'aria-checked',
    'false'
  );
  await checkboxButtonFalseDescription.click();
  await expect(checkboxButtonFalseDescription).toHaveAttribute(
    'aria-checked',
    'true'
  );
  await checkboxButtonFalseDescription.click();

  // Checkbox true state disabled should be checked and unable to be clicked
  const checkboxButtonTrueDescriptionDisabled = appFrame!.getByTestId(
    'checkbox-true-state-width-description-disabled'
  );
  await expect(checkboxButtonTrueDescriptionDisabled).toHaveAttribute(
    'aria-checked',
    'true'
  );
  // Checkbox true state invalid should be unchecked after click
  const checkboxButtonTrueDescriptionInvalid = appFrame!.getByTestId(
    'checkbox-true-state-width-description-invalid'
  );
  await expect(checkboxButtonTrueDescriptionInvalid).toHaveAttribute(
    'aria-checked',
    'true'
  );
  await checkboxButtonTrueDescriptionInvalid.click();
  await expect(checkboxButtonTrueDescriptionInvalid).toHaveAttribute(
    'aria-checked',
    'false'
  );
  await checkboxButtonTrueDescriptionInvalid.click();
  // Checkbox null state with description should be mixed at the start, checked after first click
  //  and unchecked after second click
  const checkboxButtonNullDescription = appFrame!.getByTestId(
    'checkbox-null-state-width-description'
  );
  await expect(checkboxButtonNullDescription).toHaveAttribute(
    'aria-checked',
    'mixed'
  );
  await checkboxButtonNullDescription.click();
  await expect(checkboxButtonNullDescription).toHaveAttribute(
    'aria-checked',
    'true'
  );
  await checkboxButtonNullDescription.click();
  await expect(checkboxButtonNullDescription).toHaveAttribute(
    'aria-checked',
    'false'
  );
  await checkboxButtonNullDescription.click();

  // Checkbox true state should be unchecked after click
  const checkboxButtonTrue = appFrame!.getByTestId('checkbox-true-state-width');
  await expect(checkboxButtonTrue).toHaveAttribute('aria-checked', 'true');
  await checkboxButtonTrue.click();
  await expect(checkboxButtonTrue).toHaveAttribute('aria-checked', 'false');
  await checkboxButtonTrue.click();

  // Checkbox false state should be unchecked after click
  const checkboxButtonFalse = appFrame!.getByTestId(
    'checkbox-false-state-width'
  );
  await expect(checkboxButtonFalse).toHaveAttribute('aria-checked', 'false');
  await checkboxButtonFalse.click();
  await expect(checkboxButtonFalse).toHaveAttribute('aria-checked', 'true');
  await checkboxButtonFalse.click();

  // Checkbox true state disabled should be checked and unable to be clicked
  const checkboxButtonTrueDisabled = appFrame!.getByTestId(
    'checkbox-true-state-width-disabled'
  );
  await expect(checkboxButtonTrueDisabled).toHaveAttribute(
    'aria-checked',
    'true'
  );
  // Checkbox true state invalid should be unchecked after click
  const checkboxButtonTrueInvalid = appFrame!.getByTestId(
    'checkbox-true-state-width-invalid'
  );
  await expect(checkboxButtonTrueInvalid).toHaveAttribute(
    'aria-checked',
    'true'
  );
  await checkboxButtonTrueInvalid.click();
  await expect(checkboxButtonTrueInvalid).toHaveAttribute(
    'aria-checked',
    'false'
  );
  await checkboxButtonTrueInvalid.click();

  // Checkbox null state should be mixed at the start, checked after first click
  //  and unchecked after second click
  const checkboxButtonNull = appFrame!.getByTestId('checkbox-null-state-width');
  await expect(checkboxButtonNull).toHaveAttribute('aria-checked', 'mixed');
  await checkboxButtonNull.click();
  await expect(checkboxButtonNull).toHaveAttribute('aria-checked', 'true');
  await checkboxButtonNull.click();
  await expect(checkboxButtonNull).toHaveAttribute('aria-checked', 'false');
  await checkboxButtonNull.click();

  // Switche with description aria-checked tests
  //Switch true state with description should be unchecked after click
  const switchButtonTrueDescription = appFrame!.getByTestId(
    'switch-true-state-width-description'
  );
  await expect(switchButtonTrueDescription).toHaveAttribute(
    'aria-checked',
    'true'
  );
  await switchButtonTrueDescription.click();
  await expect(switchButtonTrueDescription).toHaveAttribute(
    'aria-checked',
    'false'
  );
  await switchButtonTrueDescription.click();
  // Switch false state with description should be checked after click
  const switchButtonFalseDescription = appFrame!.getByTestId(
    'switch-false-state-width-description'
  );
  await expect(switchButtonFalseDescription).toHaveAttribute(
    'aria-checked',
    'false'
  );
  await switchButtonFalseDescription.click();
  await expect(switchButtonFalseDescription).toHaveAttribute(
    'aria-checked',
    'true'
  );
  await switchButtonFalseDescription.click();
  // Switch true state disabled should be checked and unable to be clicked
  const switchButtonTrueDescriptionDisabled = appFrame!.getByTestId(
    'switch-true-state-width-description-disabled'
  );
  await expect(switchButtonTrueDescriptionDisabled).toHaveAttribute(
    'aria-checked',
    'true'
  );
  // Switch true state invalid should be unchecked after click
  const switchButtonTrueDescriptionInvalid = appFrame!.getByTestId(
    'switch-true-state-width-description-invalid'
  );
  await expect(switchButtonTrueDescriptionInvalid).toHaveAttribute(
    'aria-checked',
    'true'
  );
  await switchButtonTrueDescriptionInvalid.click();
  await expect(switchButtonTrueDescriptionInvalid).toHaveAttribute(
    'aria-checked',
    'false'
  );
  await switchButtonTrueDescriptionInvalid.click();

  // Switches without description aria-checked tests
  // Switch true state should be unchecked after click
  const switchButtonTrue = appFrame!.getByTestId('switch-true-state-width');
  await expect(switchButtonTrue).toHaveAttribute('aria-checked', 'true');
  await switchButtonTrue.click();
  await expect(switchButtonTrue).toHaveAttribute('aria-checked', 'false');
  await switchButtonTrue.click();

  // Switch false state should be checked after click
  const switchButtonFalse = appFrame!.getByTestId('switch-false-state-width');
  await expect(switchButtonFalse).toHaveAttribute('aria-checked', 'false');
  await switchButtonFalse.click();
  await expect(switchButtonFalse).toHaveAttribute('aria-checked', 'true');
  await switchButtonFalse.click();

  // Switch true state disabled should be checked and unable to be clicked
  const switchButtonTrueDisabled = appFrame!.getByTestId(
    'switch-true-state-width-disabled'
  );
  await expect(switchButtonTrueDisabled).toHaveAttribute(
    'aria-checked',
    'true'
  );
  // Switch true state invalid should be unchecked after click
  const switchButtonTrueInvalid = appFrame!.getByTestId(
    'switch-true-state-width-invalid'
  );
  await expect(switchButtonTrueInvalid).toHaveAttribute('aria-checked', 'true');
  await switchButtonTrueInvalid.click();
  await expect(switchButtonTrueInvalid).toHaveAttribute(
    'aria-checked',
    'false'
  );
  await switchButtonTrueInvalid.click();

  // Toggles with description aria-checked tests
});
