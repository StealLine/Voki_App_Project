/** @type {import('stylelint').Config} */
export default {
  extends: ['stylelint-config-standard'],
  ignoreFiles: ['**/build/**'],
  plugins: ['stylelint-order'],
  rules: {
    'no-descending-specificity': null,
    'order/properties-order': [
      'position',
      'top',
      'right',
      'bottom',
      'left',
      'z-index',

      'display',
      'flex-direction',
      'flex-wrap',
      'justify-content',
      'align-items',
      'align-content',
      'gap',

      'width',
      'min-width',
      'max-width',
      'height',
      'min-height',
      'max-height',

      'box-sizing',

      'padding',
      'padding-top',
      'padding-right',
      'padding-bottom',
      'padding-left',

      'margin',
      'margin-top',
      'margin-right',
      'margin-bottom',
      'margin-left',

      'border',
      'border-width',
      'border-style',
      'border-color',
      'border-radius',

      'background',
      'background-color',
      'color',

      'font',
      'font-size',
      'font-weight',
      'line-height',
      'text-align',
      'text-decoration',
      'text-transform',
      'letter-spacing',

      'opacity',
      'box-shadow',
      'transition',
      'transform',
      'animation',
      'cursor'
    ]
  },
  overrides: [
    {
      files: ['**/*.svelte'],
      customSyntax: 'postcss-html',
      rules: {
        'selector-pseudo-class-no-unknown': [true, { ignorePseudoClasses: ['global', 'deep'] }],
        "property-no-vendor-prefix": null
      }
    }
  ]
};
