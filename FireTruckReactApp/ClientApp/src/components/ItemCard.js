import React from 'react';
import PropTypes from 'prop-types';
import './ItemCard.css';


const ItemCard = ({ itemData: itemData, selected, handleClickItem }) => {
    return (
        <div className="item-card" onClick={handleClickItem}>
            <div className="card-body">
                <div className="card-subtitle">{itemData.identifier}</div>
            </div>
        </div>
    );
};

ItemCard.propTypes = {
    itemData: PropTypes.shape({
        identifier: PropTypes.string.isRequired,
    }).isRequired,
    selected: PropTypes.bool.isRequired,
};

export default ItemCard;
