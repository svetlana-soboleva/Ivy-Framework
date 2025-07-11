import { test, expect, type Frame, type Page } from '@playwright/test';

// Shared setup function
async function setupBoolInputPage(page: Page): Promise<Frame | null> {
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
  await firstResult.click();

  // Wait for the page to load
  await page.waitForLoadState('networkidle');
  const appFrameElement = await page.waitForSelector(
    'iframe[src*="bool-input"]'
  );
  return await appFrameElement.contentFrame();
}

// Shared verification function for all elements
async function verifyAllElements(appFrame: Frame) {
  // Now look for the h1 in the app frame
  const h1 = appFrame.locator('h1');
  await expect(h1).toContainText('BoolInput');

  // Now look for the h2 in the app frame
  const h2 = appFrame.locator('h2');
  await expect(h2.nth(0)).toContainText('Variants');

  await expect(
    appFrame.locator('code:has-text("BoolInputs.Checkbox")')
  ).toBeVisible();
  await expect(
    appFrame.locator('code:has-text("BoolInputs.Switch")')
  ).toBeVisible();
  await expect(
    appFrame.locator('code:has-text("BoolInputs.Toggle")')
  ).toBeVisible();

  // Define test IDs for each category
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
  ];

  const switchWithoutDescriptionIds = [
    'switch-true-state-width',
    'switch-false-state-width',
    'switch-true-state-width-disabled',
    'switch-true-state-width-description-invalid',
  ];

  const toggleWithDescriptionIds = [
    'toggle-true-state-width-description',
    'toggle-false-state-width-description',
    'toggle-true-state-width-description-disabled',
    'toggle-true-state-width-description-invalid',
  ];

  const toggleWithoutDescriptionIds = [
    'toggle-true-state-width',
    'toggle-false-state-width',
    'toggle-true-state-width-disabled',
    'toggle-true-state-width-invalid',
  ];

  // Check all elements are visible
  const allTestIds = [
    ...checkboxWithDescriptionIds,
    ...checkboxWithoutDescriptionIds,
    ...switchWithDescriptionIds,
    ...switchWithoutDescriptionIds,
    ...toggleWithDescriptionIds,
    ...toggleWithoutDescriptionIds,
  ];

  for (const testId of allTestIds) {
    await expect(appFrame.getByTestId(testId)).toBeVisible();
  }
}

test.describe('Bool Input Tests', () => {
  let appFrame: Frame | null;

  test.beforeEach(async ({ page }) => {
    appFrame = await setupBoolInputPage(page);
  });

  // Helper function to test toggle behavior
  async function testToggleBehavior(
    testId: string,
    attributeName: 'aria-checked' | 'aria-pressed',
    initialState: string,
    expectedAfterClick: string
  ) {
    const element = appFrame!.getByTestId(testId);

    await expect(element).toHaveAttribute(attributeName, initialState);
    await element.click();
    await expect(element).toHaveAttribute(attributeName, expectedAfterClick);
    await element.click();
    await expect(element).toHaveAttribute(attributeName, initialState);
  }

  // Helper function to test disabled state
  async function testDisabledState(
    testId: string,
    attributeName: 'aria-checked' | 'aria-pressed',
    expectedState: string
  ) {
    const element = appFrame!.getByTestId(testId);

    await expect(element).toHaveAttribute(attributeName, expectedState);
    await expect(element).toBeDisabled();
  }

  // Helper function to test null state cycling (mixed -> true -> false -> mixed)
  async function testNullStateCycling(testId: string) {
    const element = appFrame!.getByTestId(testId);

    await expect(element).toHaveAttribute('aria-checked', 'mixed');
    await element.click();
    await expect(element).toHaveAttribute('aria-checked', 'true');
    await element.click();
    await expect(element).toHaveAttribute('aria-checked', 'false');
    await element.click();
    await expect(element).toHaveAttribute('aria-checked', 'mixed');
  }

  test.describe('Page Navigation', () => {
    test('should navigate to bool input page and verify elements', async () => {
      expect(appFrame).not.toBeNull();
      await verifyAllElements(appFrame!);
    });
  });

  test.describe('Interactive Tests', () => {
    test.describe('Checkboxes', () => {
      test.describe('With Description', () => {
        test('true state should toggle correctly', async () => {
          await testToggleBehavior(
            'checkbox-true-state-width-description',
            'aria-checked',
            'true',
            'false'
          );
        });

        test('false state should toggle correctly', async () => {
          await testToggleBehavior(
            'checkbox-false-state-width-description',
            'aria-checked',
            'false',
            'true'
          );
        });

        test('disabled state should remain checked', async () => {
          await testDisabledState(
            'checkbox-true-state-width-description-disabled',
            'aria-checked',
            'true'
          );
        });

        test('invalid state should toggle correctly', async () => {
          await testToggleBehavior(
            'checkbox-true-state-width-description-invalid',
            'aria-checked',
            'true',
            'false'
          );
        });

        test('null state should cycle through mixed -> true -> false', async () => {
          await testNullStateCycling('checkbox-null-state-width-description');
        });
      });

      test.describe('Without Description', () => {
        test('true state should toggle correctly', async () => {
          await testToggleBehavior(
            'checkbox-true-state-width',
            'aria-checked',
            'true',
            'false'
          );
        });

        test('false state should toggle correctly', async () => {
          await testToggleBehavior(
            'checkbox-false-state-width',
            'aria-checked',
            'false',
            'true'
          );
        });

        test('disabled state should remain checked', async () => {
          await testDisabledState(
            'checkbox-true-state-width-disabled',
            'aria-checked',
            'true'
          );
        });

        test('invalid state should toggle correctly', async () => {
          await testToggleBehavior(
            'checkbox-true-state-width-invalid',
            'aria-checked',
            'true',
            'false'
          );
        });

        test('null state should cycle through mixed -> true -> false', async () => {
          await testNullStateCycling('checkbox-null-state-width');
        });
      });
    });

    test.describe('Switches', () => {
      test.describe('With Description', () => {
        test('true state should toggle correctly', async () => {
          await testToggleBehavior(
            'switch-true-state-width-description',
            'aria-checked',
            'true',
            'false'
          );
        });

        test('false state should toggle correctly', async () => {
          await testToggleBehavior(
            'switch-false-state-width-description',
            'aria-checked',
            'false',
            'true'
          );
        });

        test('disabled state should remain checked', async () => {
          await testDisabledState(
            'switch-true-state-width-description-disabled',
            'aria-checked',
            'true'
          );
        });

        test('invalid state should toggle correctly', async () => {
          await testToggleBehavior(
            'switch-true-state-width-description-invalid',
            'aria-checked',
            'true',
            'false'
          );
        });
      });

      test.describe('Without Description', () => {
        test('true state should toggle correctly', async () => {
          await testToggleBehavior(
            'switch-true-state-width',
            'aria-checked',
            'true',
            'false'
          );
        });

        test('false state should toggle correctly', async () => {
          await testToggleBehavior(
            'switch-false-state-width',
            'aria-checked',
            'false',
            'true'
          );
        });

        test('disabled state should remain checked', async () => {
          await testDisabledState(
            'switch-true-state-width-disabled',
            'aria-checked',
            'true'
          );
        });

        test('invalid state should toggle correctly', async () => {
          await testToggleBehavior(
            'switch-true-state-width-invalid',
            'aria-checked',
            'true',
            'false'
          );
        });
      });
    });

    test.describe('Toggles', () => {
      test.describe('With Description', () => {
        test('true state should toggle correctly', async () => {
          await testToggleBehavior(
            'toggle-true-state-width-description',
            'aria-pressed',
            'true',
            'false'
          );
        });

        test('false state should toggle correctly', async () => {
          await testToggleBehavior(
            'toggle-false-state-width-description',
            'aria-pressed',
            'false',
            'true'
          );
        });

        test('disabled state should remain pressed', async () => {
          await testDisabledState(
            'toggle-true-state-width-description-disabled',
            'aria-pressed',
            'true'
          );
        });

        test('invalid state should toggle correctly', async () => {
          await testToggleBehavior(
            'toggle-true-state-width-description-invalid',
            'aria-pressed',
            'true',
            'false'
          );
        });
      });

      test.describe('Without Description', () => {
        test('true state should toggle correctly', async () => {
          await testToggleBehavior(
            'toggle-true-state-width',
            'aria-pressed',
            'true',
            'false'
          );
        });

        test('false state should toggle correctly', async () => {
          await testToggleBehavior(
            'toggle-false-state-width',
            'aria-pressed',
            'false',
            'true'
          );
        });

        test('disabled state should remain pressed', async () => {
          await testDisabledState(
            'toggle-true-state-width-disabled',
            'aria-pressed',
            'true'
          );
        });

        test('invalid state should toggle correctly', async () => {
          await testToggleBehavior(
            'toggle-true-state-width-invalid',
            'aria-pressed',
            'true',
            'false'
          );
        });
      });
    });
  });
});
