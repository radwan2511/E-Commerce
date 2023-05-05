import React, { useState, useEffect } from 'react';
import { Container, ListGroup, Button } from 'react-bootstrap';
import { CartItem } from '../components/CartItem.tsx';
import { useNavigate } from "react-router-dom";
import { Item } from '../models/Shop.tsx';
import SystemNotInit from './SystemNotInit.tsx';
import { handleGetDetailsOnCart } from '../actions/GuestActions.tsx';

function CartPage(props) {

  const navigate = useNavigate();
  const [cartItems, setCartItems] = useState<Item[]>([]);
  const [totalPrice, setTotalPrice] = useState(0);

  const calculatePrice=()=>{
    const sum = cartItems.reduce((acc, item) => acc + (item.price*item.count), 0);
    console.log("pricee "+sum);
    setTotalPrice(sum);
  }
  
  const getShoppingCartItems=()=>{
    handleGetDetailsOnCart(props.id).then(
      value => {
        setCartItems(value as Item[]);
        calculatePrice();
      })
      .catch(error => alert(error));
  }

  useEffect(() => {
    getShoppingCartItems();
    calculatePrice();
  }, []);

  useEffect(() => {
    calculatePrice();
  }, [cartItems]);

  const shoppingCartChanged = () => {
    getShoppingCartItems();
    calculatePrice(); 
  };

  


  return (
    props.isInit?
    (<Container className="my-5">
      {cartItems.length === 0 ? (
        <p>Your cart is empty.</p>
      ) : (
        <div>
          <ListGroup>
            {cartItems.map((item) => (
              <CartItem key={item.itemId} item={item} id={props.id} onShoppingCartChanged={shoppingCartChanged}/>
            ))}
          </ListGroup>
          <hr />
          <h4>Total Price: ${totalPrice}</h4>
          <Button variant="success" size="lg" onClick={() => navigate("/PaymentPage")}>Checkout Now</Button>
        </div>
      )}
    </Container>):(<SystemNotInit/>)
  );
}

export default CartPage;
