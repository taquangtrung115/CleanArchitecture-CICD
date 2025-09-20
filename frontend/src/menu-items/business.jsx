// assets
import { ShoppingCartOutlined } from '@ant-design/icons';

const icons = {
  ShoppingCartOutlined
};

const business = {
  id: 'business',
  title: 'Business Management',
  type: 'group',
  children: [
    {
      id: 'product-management',
      title: 'Products',
      type: 'item',
      url: '/admin/products',
      icon: icons.ShoppingCartOutlined
    }
  ]
};

export default business;
