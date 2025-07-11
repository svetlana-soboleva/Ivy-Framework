import { test, expect, type Frame } from '@playwright/test';
test.describe('Bool Input Page Navigation', () => {
  test('should navigate to bool input page and verify elements', async ({
    page,
  }) => {
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

    // Checkboxes with description visible tests
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

    // Checkboxes without description visible tests
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

    // Switches with description visible tests
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

    // Switches without description visible tests
    await expect(
      appFrame!.getByTestId('switch-true-state-width')
    ).toBeVisible();
    await expect(
      appFrame!.getByTestId('switch-false-state-width')
    ).toBeVisible();
    await expect(
      appFrame!.getByTestId('switch-true-state-width-disabled')
    ).toBeVisible();
    await expect(
      appFrame!.getByTestId('switch-true-state-width-description-invalid')
    ).toBeVisible();

    // Toggles with description visible tests
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

    // Toggles without description visible tests
    await expect(
      appFrame!.getByTestId('toggle-true-state-width')
    ).toBeVisible();
    await expect(
      appFrame!.getByTestId('toggle-false-state-width')
    ).toBeVisible();
    await expect(
      appFrame!.getByTestId('toggle-true-state-width-disabled')
    ).toBeVisible();
    await expect(
      appFrame!.getByTestId('toggle-true-state-width-invalid')
    ).toBeVisible();
  });
});

test.describe('Bool Input Interactive Tests', () => {
  let appFrame: Frame | null;

  test.beforeEach(async ({ page }) => {
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
    appFrame = await appFrameElement.contentFrame();
  });

  test.describe('Checkboxes', () => {
    test.describe('With Description', () => {
      test('true state should toggle correctly', async () => {
        const checkbox = appFrame!.getByTestId(
          'checkbox-true-state-width-description'
        );

        await expect(checkbox).toHaveAttribute('aria-checked', 'true');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'false');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'true');
      });

      test('false state should toggle correctly', async () => {
        const checkbox = appFrame!.getByTestId(
          'checkbox-false-state-width-description'
        );

        await expect(checkbox).toHaveAttribute('aria-checked', 'false');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'true');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'false');
      });

      test('disabled state should remain checked', async () => {
        const checkbox = appFrame!.getByTestId(
          'checkbox-true-state-width-description-disabled'
        );

        await expect(checkbox).toHaveAttribute('aria-checked', 'true');
        await expect(checkbox).toBeDisabled();
        // Note: Disabled checkboxes should not change state when clicked
      });

      test('invalid state should toggle correctly', async () => {
        const checkbox = appFrame!.getByTestId(
          'checkbox-true-state-width-description-invalid'
        );

        await expect(checkbox).toHaveAttribute('aria-checked', 'true');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'false');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'true');
      });

      test('null state should cycle through mixed -> true -> false', async () => {
        const checkbox = appFrame!.getByTestId(
          'checkbox-null-state-width-description'
        );

        await expect(checkbox).toHaveAttribute('aria-checked', 'mixed');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'true');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'false');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'mixed');
      });
    });

    test.describe('Without Description', () => {
      test('true state should toggle correctly', async () => {
        const checkbox = appFrame!.getByTestId('checkbox-true-state-width');

        await expect(checkbox).toHaveAttribute('aria-checked', 'true');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'false');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'true');
      });

      test('false state should toggle correctly', async () => {
        const checkbox = appFrame!.getByTestId('checkbox-false-state-width');

        await expect(checkbox).toHaveAttribute('aria-checked', 'false');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'true');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'false');
      });

      test('disabled state should remain checked', async () => {
        const checkbox = appFrame!.getByTestId(
          'checkbox-true-state-width-disabled'
        );

        await expect(checkbox).toHaveAttribute('aria-checked', 'true');
        await expect(checkbox).toBeDisabled();
      });

      test('invalid state should toggle correctly', async () => {
        const checkbox = appFrame!.getByTestId(
          'checkbox-true-state-width-invalid'
        );

        await expect(checkbox).toHaveAttribute('aria-checked', 'true');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'false');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'true');
      });

      test('null state should cycle through mixed -> true -> false', async () => {
        const checkbox = appFrame!.getByTestId('checkbox-null-state-width');

        await expect(checkbox).toHaveAttribute('aria-checked', 'mixed');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'true');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'false');
        await checkbox.click();
        await expect(checkbox).toHaveAttribute('aria-checked', 'mixed');
      });
    });
  });

  test.describe('Switches', () => {
    test.describe('With Description', () => {
      test('true state should toggle correctly', async () => {
        const switchButton = appFrame!.getByTestId(
          'switch-true-state-width-description'
        );

        await expect(switchButton).toHaveAttribute('aria-checked', 'true');
        await switchButton.click();
        await expect(switchButton).toHaveAttribute('aria-checked', 'false');
        await switchButton.click();
        await expect(switchButton).toHaveAttribute('aria-checked', 'true');
      });

      test('false state should toggle correctly', async () => {
        const switchButton = appFrame!.getByTestId(
          'switch-false-state-width-description'
        );

        await expect(switchButton).toHaveAttribute('aria-checked', 'false');
        await switchButton.click();
        await expect(switchButton).toHaveAttribute('aria-checked', 'true');
        await switchButton.click();
        await expect(switchButton).toHaveAttribute('aria-checked', 'false');
      });

      test('disabled state should remain checked', async () => {
        const switchButton = appFrame!.getByTestId(
          'switch-true-state-width-description-disabled'
        );

        await expect(switchButton).toHaveAttribute('aria-checked', 'true');
        await expect(switchButton).toBeDisabled();
      });

      test('invalid state should toggle correctly', async () => {
        const switchButton = appFrame!.getByTestId(
          'switch-true-state-width-description-invalid'
        );

        await expect(switchButton).toHaveAttribute('aria-checked', 'true');
        await switchButton.click();
        await expect(switchButton).toHaveAttribute('aria-checked', 'false');
        await switchButton.click();
        await expect(switchButton).toHaveAttribute('aria-checked', 'true');
      });
    });

    test.describe('Without Description', () => {
      test('true state should toggle correctly', async () => {
        const switchButton = appFrame!.getByTestId('switch-true-state-width');

        await expect(switchButton).toHaveAttribute('aria-checked', 'true');
        await switchButton.click();
        await expect(switchButton).toHaveAttribute('aria-checked', 'false');
        await switchButton.click();
        await expect(switchButton).toHaveAttribute('aria-checked', 'true');
      });

      test('false state should toggle correctly', async () => {
        const switchButton = appFrame!.getByTestId('switch-false-state-width');

        await expect(switchButton).toHaveAttribute('aria-checked', 'false');
        await switchButton.click();
        await expect(switchButton).toHaveAttribute('aria-checked', 'true');
        await switchButton.click();
        await expect(switchButton).toHaveAttribute('aria-checked', 'false');
      });

      test('disabled state should remain checked', async () => {
        const switchButton = appFrame!.getByTestId(
          'switch-true-state-width-disabled'
        );

        await expect(switchButton).toHaveAttribute('aria-checked', 'true');
        await expect(switchButton).toBeDisabled();
      });

      test('invalid state should toggle correctly', async () => {
        const switchButton = appFrame!.getByTestId(
          'switch-true-state-width-invalid'
        );

        await expect(switchButton).toHaveAttribute('aria-checked', 'true');
        await switchButton.click();
        await expect(switchButton).toHaveAttribute('aria-checked', 'false');
        await switchButton.click();
        await expect(switchButton).toHaveAttribute('aria-checked', 'true');
      });
    });
  });

  test.describe('Toggles', () => {
    test.describe('With Description', () => {
      test('true state should toggle correctly', async () => {
        const toggleButton = appFrame!.getByTestId(
          'toggle-true-state-width-description'
        );

        await expect(toggleButton).toHaveAttribute('aria-pressed', 'true');
        await toggleButton.click();
        await expect(toggleButton).toHaveAttribute('aria-pressed', 'false');
        await toggleButton.click();
        await expect(toggleButton).toHaveAttribute('aria-pressed', 'true');
      });

      test('false state should toggle correctly', async () => {
        const toggleButton = appFrame!.getByTestId(
          'toggle-false-state-width-description'
        );

        await expect(toggleButton).toHaveAttribute('aria-pressed', 'false');
        await toggleButton.click();
        await expect(toggleButton).toHaveAttribute('aria-pressed', 'true');
        await toggleButton.click();
        await expect(toggleButton).toHaveAttribute('aria-pressed', 'false');
      });

      test('disabled state should remain pressed', async () => {
        const toggleButton = appFrame!.getByTestId(
          'toggle-true-state-width-description-disabled'
        );

        await expect(toggleButton).toHaveAttribute('aria-pressed', 'true');
        await expect(toggleButton).toBeDisabled();
      });

      test('invalid state should toggle correctly', async () => {
        const toggleButton = appFrame!.getByTestId(
          'toggle-true-state-width-description-invalid'
        );

        await expect(toggleButton).toHaveAttribute('aria-pressed', 'true');
        await toggleButton.click();
        await expect(toggleButton).toHaveAttribute('aria-pressed', 'false');
        await toggleButton.click();
        await expect(toggleButton).toHaveAttribute('aria-pressed', 'true');
      });
    });

    test.describe('Without Description', () => {
      test('true state should toggle correctly', async () => {
        const toggleButton = appFrame!.getByTestId('toggle-true-state-width');

        await expect(toggleButton).toHaveAttribute('aria-pressed', 'true');
        await toggleButton.click();
        await expect(toggleButton).toHaveAttribute('aria-pressed', 'false');
        await toggleButton.click();
        await expect(toggleButton).toHaveAttribute('aria-pressed', 'true');
      });

      test('false state should toggle correctly', async () => {
        const toggleButton = appFrame!.getByTestId('toggle-false-state-width');

        await expect(toggleButton).toHaveAttribute('aria-pressed', 'false');
        await toggleButton.click();
        await expect(toggleButton).toHaveAttribute('aria-pressed', 'true');
        await toggleButton.click();
        await expect(toggleButton).toHaveAttribute('aria-pressed', 'false');
      });

      test('disabled state should remain pressed', async () => {
        const toggleButton = appFrame!.getByTestId(
          'toggle-true-state-width-disabled'
        );

        await expect(toggleButton).toHaveAttribute('aria-pressed', 'true');
        await expect(toggleButton).toBeDisabled();
      });

      test('invalid state should toggle correctly', async () => {
        const toggleButton = appFrame!.getByTestId(
          'toggle-true-state-width-invalid'
        );

        await expect(toggleButton).toHaveAttribute('aria-pressed', 'true');
        await toggleButton.click();
        await expect(toggleButton).toHaveAttribute('aria-pressed', 'false');
        await toggleButton.click();
        await expect(toggleButton).toHaveAttribute('aria-pressed', 'true');
      });
    });
  });
});
